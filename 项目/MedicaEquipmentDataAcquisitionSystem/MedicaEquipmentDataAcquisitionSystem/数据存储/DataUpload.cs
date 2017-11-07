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
        public static Func<string> VerifyAsyn = Verify;
        public static Func<string, string, string> MonitorAsyn = Monitor;
        public static Func<string, string, string> ReportAsyn = Report;
        public static Action<string> resultAnalyseAsyn = resultAnalyse;
        //public static string ServerPath = "http://test.zy360.com/";
        public static string ServerPath = "http://tcmh.zy360.com/";
        static byte[] KEY, IV;
        static DataUpload()
        {
            KEY = Encoding.ASCII.GetBytes("xEu1p;LP7iZmxKyTtA*HV9pS");
            IV = Encoding.ASCII.GetBytes("j0?2UK+p");
            string path = ToolAPI.INIOperate.IniReadValue("Base", "ServerPath", Application.StartupPath + "\\Config.ini");
            if(!string.IsNullOrEmpty(path))
            {
                ServerPath = path;
            }
        }
        #region 软件安装验证
        /// <summary>
        /// 软件安装验证
        /// </summary>
        /// <returns></returns>
        static string Verify()
        {
            try
            {
                string devicesn_uuid = IdentityVerification.GetMacAddress();
                //string devicesn = "RK-73106856";//测试用
                string devicesn = MainStatic.devicesn;
                //string code = INIOperate.IniReadValue("Base", "sn", Application.StartupPath + "\\Config.ini");
                //string code = "3A04MKRAE3WP";//测试用
                string code = MainStatic.code;
                VerifyC vc = new VerifyC();
                vc.devicesn = devicesn;
                vc.uuid = devicesn_uuid;
                vc.code = code;
                vc.timestamp = HTTP_Base.GetTimeStamp();
                vc.nonce = RandomString();
                string Json = Newtonsoft.Json.JsonConvert.SerializeObject(vc);
                ToolAPI.XMLOperation.WriteLogXmlNoTail("软件安装验证Json拼接", Json);
                byte[] data = Encoding.UTF8.GetBytes(Json);
                byte[] des3 = Des3.Des3EncodeCBC(KEY, IV, data);
                string EncryptStr = Convert.ToBase64String(des3);
                HTTP_Base hb = new HTTP_Base(ServerPath+"tcmd/Cclient/www/Rs/verify");
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("param", UrlEncode(EncryptStr));
                string result = hb.Http_Request(dic, vc.GetDictionary());
                ToolAPI.XMLOperation.WriteLogXmlNoTail("软件安装验证结果应答", result);
                resultAnalyseAsyn.BeginInvoke(result, null, null);
                return result;
            }
            catch (Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("软件安装验证异常", ex.Message);
                return ex.Message;
            }
        }
        [Serializable]
        public class VerifyC
        {
            public string code { get; set; }
            public string devicesn { get; set; }
            public string nonce { get; set; }
            public string timestamp { get; set; }
            public string uuid { get; set; }
            public Dictionary<string, string> GetDictionary()
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("code", code ?? "");
                dic.Add("devicesn", devicesn ?? "");
                dic.Add("nonce", nonce ?? "");
                dic.Add("timestamp", timestamp ?? "");
                dic.Add("uuid", uuid ?? "");
                return dic;
            }
        }
        #endregion
        #region 设备监控
        /// <summary>
        /// 设备监控
        /// </summary>
        /// <param name="code">设备连接是否正常</param>
        /// <returns></returns>
        static string Monitor(string code, string devicesnTemp)
        {
            try
            {
                //string devicesn = MainStatic.devicesn; 
                //string devicesn = "RK-73106856";//测试用
                string devicesn = devicesnTemp;
                //string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                string version = INIOperate.IniReadValue("Base", "version", Application.StartupPath + "\\Config.ini");
                MonitorC mc = new MonitorC();
                mc.devicesn = devicesn;
                mc.version = version;
                mc.code = code;
                mc.timestamp = HTTP_Base.GetTimeStamp();
                mc.nonce = RandomString();
                string Json = Newtonsoft.Json.JsonConvert.SerializeObject(mc);
                ToolAPI.XMLOperation.WriteLogXmlNoTail("设备监控Json拼接", Json);
                byte[] data = Encoding.UTF8.GetBytes(Json);
                byte[] des3 = Des3.Des3EncodeCBC(KEY, IV, data);
                string EncryptStr = Convert.ToBase64String(des3);
                HTTP_Base hb = new HTTP_Base(ServerPath + "tcmd/Cclient/www/Rs/monitor");
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("param", UrlEncode(EncryptStr));
                string result = hb.Http_Request(dic, mc.GetDictionary());
                ToolAPI.XMLOperation.WriteLogXmlNoTail("设备监控结果应答", result);
                resultAnalyseAsyn.BeginInvoke(result, null, null);
                return result;
            }
            catch (Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("设备监控异常", ex.Message);
                return ex.Message;
            }
        }
        [Serializable]
        public class MonitorC
        {
            public string code { get; set; }
            public string devicesn { get; set; }
            public string nonce { get; set; }
            public string timestamp { get; set; }
            public string version { get; set; }
            public Dictionary<string, string> GetDictionary()
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("code", code ?? "");
                dic.Add("devicesn", devicesn ?? "");
                dic.Add("nonce", nonce ?? "");
                dic.Add("timestamp", timestamp ?? "");
                dic.Add("version", version ?? "");
                return dic;
            }
        }
        #endregion
        #region 上报数据
        /// <summary>
        /// 上报数据
        /// </summary>
        /// <param name="datas">实时数据JSON串</param>
        /// <returns></returns>
        static string Report(string datas, string devicesnTemp)
        {
            try
            {
                string devicesn = devicesnTemp;
                //if(SnType=="Bm21")
                //string devicesn = "SN-15046750";//测试用
                //else if(SnType=="Bm200")
                //string devicesn = "JYGZ-01";//测试用
                //else
                //string devicesn = "RK-73106856";//测试用
                ReportC rc = new ReportC();
                rc.devicesn = devicesn;
                rc.datas = datas;
                rc.timestamp = HTTP_Base.GetTimeStamp();
                rc.nonce = RandomString();
                string Json = Newtonsoft.Json.JsonConvert.SerializeObject(rc);
                ToolAPI.XMLOperation.WriteLogXmlNoTail("上报数据Json拼接", Json);
                //byte[] data = Encoding.Default.GetBytes(Json);
                byte[] data = Encoding.UTF8.GetBytes(Json);
                byte[] des3 = Des3.Des3EncodeCBC(KEY, IV, data);
                string EncryptStr = Convert.ToBase64String(des3);
                HTTP_Base hb = new HTTP_Base(ServerPath+"tcmd/Cclient/www/Rs/report");
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("param", UrlEncode(EncryptStr));
                string result = hb.Http_Request(dic, rc.GetDictionary());
                ToolAPI.XMLOperation.WriteLogXmlNoTail("上报数据结果应答", result);
                resultAnalyseAsyn.BeginInvoke(result, null, null);
                return result;
            }
            catch (Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("上报数据异常", ex.Message);
                return ex.Message;
            }
        }
        [Serializable]
        public class ReportC
        {
            public string datas { get; set; }
            public string devicesn { get; set; }
            public string nonce { get; set; }
            public string timestamp { get; set; }
            public Dictionary<string, string> GetDictionary()
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("datas", datas ?? "");
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
        #endregion

    }
}
