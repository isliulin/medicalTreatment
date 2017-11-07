using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ToolAPI;

namespace MEDAS
{
    public class DataUpload
    {
        public static Action<string> resultAnalyseAsyn = resultAnalyse;
        static byte[] KEY, IV;
        //public static string ServerPath = "http://test.zy360.com/";
        public static string ServerPath = "http://tcmh.zy360.com/";
        static DataUpload()
        {
            KEY = Encoding.ASCII.GetBytes("xEu1p;LP7iZmxKyTtA*HV9pS");
            IV = Encoding.ASCII.GetBytes("j0?2UK+p");
            string path = ToolAPI.INIOperate.IniReadValue("Base", "ServerPath", Application.StartupPath + "\\Config.ini");
            if (!string.IsNullOrEmpty(path))
            {
                ServerPath = path;
            }
        }
        #region 版本号
        /// <summary>
        /// 版本号
        /// </summary>
        /// <returns></returns>
        static public  string Version()
        {
            try
            {
                //string devicesn = "RK-73106856";//测试用
                string devicesntemp = ToolAPI.INIOperate.IniReadValue("Base", "devicesn", Application.StartupPath + "\\Config.ini");
                string devicesn = DeEncrypt(devicesntemp).Replace("\0", "");
                VersionC vc = new VersionC();
                vc.devicesn = devicesn;
                vc.timestamp = HTTP_Base.GetTimeStamp();
                vc.nonce = RandomString();
                string Json = Newtonsoft.Json.JsonConvert.SerializeObject(vc);
                ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "版本号Json拼接", Json);
                byte[] data = Encoding.Default.GetBytes(Json);
                byte[] des3 = Des3.Des3EncodeCBC(KEY, IV, data);
                string EncryptStr = Convert.ToBase64String(des3);
                HTTP_Base hb = new HTTP_Base(ServerPath + "tcmd/Cclient/www/Rs/version");
                //HTTP_Base hb = new HTTP_Base("http://localhost:13208/test.ashx");//测试用
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("param", UrlEncode(EncryptStr));
                string result = hb.Http_Request(dic, vc.GetDictionary());
                ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "版本号结果应答", result);
                resultAnalyseAsyn.BeginInvoke(result, null, null);
                return result;
            }
            catch (Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "版本号异常", ex.Message);
                return ex.Message;
            }
        }
        [Serializable]
        public class VersionC
        {
            public string devicesn { get; set; }
            public string nonce { get; set; }
            public string timestamp { get; set; }
            public Dictionary<string, string> GetDictionary()
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("devicesn", devicesn ?? "");
                dic.Add("nonce", nonce ?? "");
                dic.Add("timestamp", timestamp ?? "");
                return dic;
            }
        }
        #endregion
        #region 辅助方法
        public static void resultAnalyse(string result)
        {
            try
            {
                result = result.Replace("{", "").Replace("}", "").Replace("\"", "");
                string[] resultStr = result.Split(',');
                if (resultStr[0] != "code:200")
                {
                    string content = "";
                    foreach (string str in resultStr)
                    {
                        if (str.Contains("message"))
                        {
                            string[] mes = str.Split(':');
                            string[] con = mes[1].Split('\\');
                            foreach (string constr in con)
                            {
                                if (constr[0] == 'u')
                                    content += Unicode2String("\\" + constr);
                                else content += constr;
                            }
                        }
                    }
                    MessageBox.Show(content);
                }
            }
            catch (Exception) { }
        }
        static string String2Unicode(string source)
        {
            var bytes = Encoding.Unicode.GetBytes(source);
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Unicode转字符串
        /// </summary>
        /// <param name="source">经过Unicode编码的字符串</param>
        /// <returns>正常字符串</returns>
        static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(source, x => Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)).ToString());
        }

        //随机字符的生成器
        static string RandomString()
        {
            System.Security.Cryptography.RNGCryptoServiceProvider csp = new System.Security.Cryptography.RNGCryptoServiceProvider();
            byte[] byteCsp = new byte[4];
            csp.GetBytes(byteCsp);
            return BitConverter.ToString(byteCsp).Replace("-", "");
        }

        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }

        //解密
        static string DeEncrypt(string desString)
        {
            try
            {
                string mac = IdentityVerification.GetMacAddress();
                string baseString = "000000000000000000000000";
                int length = mac.Length;
                mac = length > 24 ? mac.Substring(0, 24) : mac + baseString.Substring(0, 24 - length);
                byte[] KEY = Encoding.ASCII.GetBytes(mac);
                byte[] IV = Encoding.ASCII.GetBytes("j0?2UK+p");
                byte[] c = Convert.FromBase64String(desString);
                byte[] des3result = Des3.Des3DecodeCBC(KEY, IV, c);
                string result = Encoding.UTF8.GetString(des3result);
                return result;
            }
            catch (Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("软件安装验证解密异常", ex.Message + ex.StackTrace);
                return "";
            }
        }
        #endregion

    }
}
