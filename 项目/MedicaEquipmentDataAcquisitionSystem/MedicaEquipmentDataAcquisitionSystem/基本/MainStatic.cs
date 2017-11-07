using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace MEDAS
{
    static class MainStatic
    {
        public static string devicesn { set; get; }
        public static string code { set; get; }
        public static Dictionary<string, string> devicesnListDic = new Dictionary<string, string>();
        public static object syncHeartbeat = new object();
        static MainStatic()
        {
            string devicesntemp = ToolAPI.INIOperate.IniReadValue("Base", "devicesn", Application.StartupPath + "\\Config.ini");
            string codetemp = ToolAPI.INIOperate.IniReadValue("Base", "code", Application.StartupPath + "\\Config.ini");
            devicesn = DeEncrypt(devicesntemp).Replace("\0", "").Replace("\n", "").Trim();
            code = DeEncrypt(codetemp).Replace("\0", "").Replace("\n", "").Trim();

            string devicesnList = ToolAPI.INIOperate.IniReadValue("Base", "devicesnList", Application.StartupPath + "\\Config.ini");
            string[] devicesnListTrem = devicesnList.Split(',');
            if (devicesnListTrem.Length>0)
            foreach(string trem in devicesnListTrem)
            {
                string[] tremAry = trem.Split('&');
                if (tremAry.Length>=2)
                devicesnListDic.Add(tremAry[0], tremAry[1]);
            }
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


        public static bool Update_DeviceSn(string devNo, string e)
        {
            lock (syncHeartbeat)
            {
                if (devicesnListDic.ContainsKey(devNo))
                {
                    string valueTemp = devicesnListDic[devNo];
                    devicesnListDic[devNo] = e;
                    return valueTemp != e;
                }
                else
                {
                    devicesnListDic.Add(devNo, e);
                    return true;
                }
            }
        }

        public static string GetDeviceSn(string key)
        {
            lock (syncHeartbeat)
            {
                if (devicesnListDic.ContainsKey(key))
                    return devicesnListDic[key];
                else
                    return "";
            }
        }
        public static Dictionary<string, string> Clone_devicesnListDic()
        {
            Dictionary<string, string> result = null;
            lock (syncHeartbeat)
            {
                result = Clone(devicesnListDic);
            }
            return result;
        }
        private static Dictionary<string, string> Clone(Dictionary<string, string> obj)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, obj);
                memoryStream.Position = 0;
                return formatter.Deserialize(memoryStream) as Dictionary<string, string>;
            }
            catch (Exception ex) { ToolAPI.XMLOperation.WriteLogXmlNoTail("devicesnListDic深拷贝出现异常", ex.Message); return null; }
        }
    }
}
