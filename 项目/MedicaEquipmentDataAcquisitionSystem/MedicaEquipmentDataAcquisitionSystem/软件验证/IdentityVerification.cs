using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;
using ToolAPI;


namespace MEDAS
{
    public class IdentityVerification
    {
        public static  string GetMacAddress()
        {
            try
            {
                //获取网卡硬件地址 
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
        }

        public static bool isPermissions()
        {
            try
            {
                string mac = "xdtc" + GetMacAddress();
                string sn = GETSn(mac);
                string snConfig = INIOperate.IniReadValue("Base", "sn", Application.StartupPath + "\\Config.ini");
                if (sn.Equals(snConfig))
                    return true;
                else
                    return false;
            }
            catch(Exception)
            {
                return false;
            }
        }

        #region 计算Mac对应的sn
        private static string GETSn(string sn)
        {
            if (!string.IsNullOrEmpty(sn))
            {
                int result = 0;
                //基准函数  
                for(int i=0;i<sn.Length;i++)
                {
                    try
                    {
                        result += computeResult(sn[i]) * computeResult(i);
                    }
                    catch(Exception )
                    {
                        result -= computeResult(sn[i]) * computeResult(i);
                    }
                }
                return result.ToString();
            }
            else return "-1";
        }

        private static int computeResult(int i)
        {
            //(3*a + 7)
            try
            {
                return 3 * i + 7;
            }
            catch (Exception )
            {
                return 1;
            }
        }
        #endregion
    }
}
