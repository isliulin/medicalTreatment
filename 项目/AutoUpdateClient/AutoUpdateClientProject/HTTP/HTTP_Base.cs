using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using ToolAPI;

namespace MEDAS
{
    public class HTTP_Base
    {
        HttpWebRequest rq;
        string URL = "";
        byte[] KEY ;
        byte[] IV ;
        public HTTP_Base(string url)
        {
            rq = (HttpWebRequest)WebRequest.Create(url);
            rq.Method = "POST";
            URL = url;
            KEY = Encoding.ASCII.GetBytes("xEu1p;LP7iZmxKyTtA*HV9pS");
            IV = Encoding.ASCII.GetBytes("j0?2UK+p");
        }


        #region HTTP Header 传参
        public void HTTP_Header(Dictionary<string, string> dic)
        {
            SetHeaderValue(rq.Headers, "X-BJ-C-AccessKeyID", "fGPeLhMkEbfsKfzCX24f");//Access Key ID
            //设备唯一号
            string did = IdentityVerification.GetMacAddress();
            SetHeaderValue(rq.Headers, "X-BJ-C-DeviceID", did);//设备唯一号（UUID）
            //时间戳
            string TimeStamp = GetTimeStamp();
            SetHeaderValue(rq.Headers, "X-BJ-C-Timestamp", TimeStamp);//时间戳
            //版本号
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            SetHeaderValue(rq.Headers, "X-BJ-C-Version", version);//版本号
            //签名串的拼接
            //StringToSign = timestamp + deviceid + AccessKey + version + url(不含参数) + (参数name+值value)按字典序拼接 + Access Key ID
            StringBuilder StringToSign = new StringBuilder();
            StringToSign.Append(TimeStamp).Append(did).Append("CpDRdMFnsyNaD9YQpNaLpqmnKfRfikpWTJNthws2").Append(version).Append(URL);
            StringBuilder body = new StringBuilder();
            foreach (var item in dic)
            {
                body.Append(item.Key);
                body.Append(item.Value.Length>400?item.Value.Substring(0,400):item.Value);
            }
            StringToSign.Append(body.ToString()).Append("fGPeLhMkEbfsKfzCX24f");
            string StringToSignStr = StringToSign.ToString();
            byte[] HMAC_SHA256Obj = HMAC_SHA256.CreateToken(StringToSignStr,"vPFonKCCmDfxcCicRvXTTpY2muXcMhL9NmwheBbX");
            string Signature = ConvertData.ToHexString(HMAC_SHA256Obj, 0, HMAC_SHA256Obj.Length);
            SetHeaderValue(rq.Headers, "X-BJ-C-Signature", Signature);//签名串
        }

          public void HTTP_Header_test()
        {
            SetHeaderValue(rq.Headers, "X-BJ-C-AccessKeyID", "fGPeLhMkEbfsKfzCX24f");//Access Key ID
            //设备唯一号
            SetHeaderValue(rq.Headers, "X-BJ-C-DeviceID", "EA6DEFFC-3A79-5396-A833-C837929C79C3");//设备唯一号（UUID）
            //时间戳
            SetHeaderValue(rq.Headers, "X-BJ-C-Timestamp", "1499050943");//时间戳
            //版本号
            SetHeaderValue(rq.Headers, "X-BJ-C-Version", "1.0");//版本号
            //签名串的拼接
            //StringToSign = timestamp + deviceid + AccessKey + version + url(不含参数) + (参数name+值value)按字典序拼接 + Access Key ID
            StringBuilder StringToSign = new StringBuilder();
            StringToSign.Append("1499050943").Append("EA6DEFFC-3A79-5396-A833-C837929C79C3").Append("CpDRdMFnsyNaD9YQpNaLpqmnKfRfikpWTJNthws2").Append("1.0").Append("http://test.zy360.com/tcmd/Cclient/www/Rs/monitor");
            StringToSign.Append("code10001devicesnRK-73106856noncefuhs0i2timestamp1499050943version1.0.1").Append("fGPeLhMkEbfsKfzCX24f");
            string StringToSignStr = StringToSign.ToString();
            if (StringToSignStr == "1499050943EA6DEFFC-3A79-5396-A833-C837929C79C3CpDRdMFnsyNaD9YQpNaLpqmnKfRfikpWTJNthws21.0http://test.zy360.com/tcmd/Cclient/www/Rs/monitorcode10001devicesnRK-73106856noncefuhs0i2timestamp1499050943version1.0.1fGPeLhMkEbfsKfzCX24f")
            {
                byte[] HMAC_SHA256Obj = HMAC_SHA256.CreateToken(StringToSignStr,"vPFonKCCmDfxcCicRvXTTpY2muXcMhL9NmwheBbX");
                string utf8 = Encoding.UTF8.GetString(HMAC_SHA256Obj);
                string Signature = ConvertData.ToHexString(HMAC_SHA256Obj, 0, HMAC_SHA256Obj.Length);
                SetHeaderValue(rq.Headers, "X-BJ-C-Signature", Signature);//签名串
            }
        }
        #endregion

        #region HTTP Body 传参
        public void HTTP_Body(Dictionary<string, string> dic)
        {
            #region 添加Post 参数
            rq.ContentType = "application/x-www-form-urlencoded";
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
            rq.ContentLength = data.Length;
            using (Stream reqStream = rq.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
        }
        #endregion

        #region 请求并得到结果
        public string Http_Request(Dictionary<string, string> dic, Dictionary<string, string> dicHeard)
        {
            HTTP_Header(dicHeard);
            //HTTP_Header_test();
            HTTP_Body(dic);
            HttpWebResponse resp = (HttpWebResponse)rq.GetResponse();
            using (Stream stream = resp.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.Default);
                string responseString = reader.ReadToEnd();
                 byte[] c = Convert.FromBase64String(responseString);
                 byte[] des3result = Des3.Des3DecodeCBC(KEY, IV, c);
                 string result = Encoding.UTF8.GetString(des3result);
                 return result;
            }
        }
        #endregion

        #region 其它 辅助方法
        //头部组装
        public  void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection", BindingFlags.Instance | BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }
        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }
        /// <summary>  
        /// 指定Post地址使用Get 方式获取全部字符串  
        /// </summary>  
        /// <param name="url">请求后台地址</param>  
        /// <returns></returns>  
        public static string Post(string url)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取内容  
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        } 

        /// <summary>  
        /// 指定Post地址使用Get 方式获取全部字符串  
        /// </summary>  
        /// <param name="url">请求后台地址</param>  
        /// <returns></returns>  
        public static string Post(string url, Dictionary<string, string> dic)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            #region 添加Post 参数
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容  
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        /// <summary>  
        /// 指定Post地址使用Get 方式获取全部字符串  
        /// </summary>  
        /// <param name="url">请求后台地址</param>  
        /// <param name="content">Post提交数据内容(utf-8编码的)</param>  
        /// <returns></returns>  
        public static string Post(string url, string content)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

            #region 添加Post 参数
            byte[] data = Encoding.UTF8.GetBytes(content);
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容  
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
        #endregion
    }
}
