using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using MEDAS;
using ToolAPI;

namespace GuideInstall
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll")]
        public static extern uint WinExec(string lpCmdLine, uint uCmdShow);

        public Form1()
        {
            InitializeComponent();
        }

        //确定
        private void button1_Click(object sender, EventArgs e)
        {
            //检查输入不能为空
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                //加密
                string devicesn = Encrypt(textBox1.Text);
                string code = Encrypt(textBox2.Text);
                //写入配置文件
                INIOperate.IniWriteValue("Base", "devicesn", devicesn, Application.StartupPath + "\\Config.ini");
                INIOperate.IniWriteValue("Base", "code", code, Application.StartupPath + "\\Config.ini");
                ToolAPI.XMLOperation.WriteLogXmlNoTail("得到devicesn和code成功", devicesn + "   " + code);
                DeleteApplication();
            }
            else
            {
                MessageBox.Show("输入值为空");
            }

           
        }

        string  Encrypt(string desString)
        {
            try
            {
                string mac = IdentityVerification.GetMacAddress();
                string baseString = "000000000000000000000000";
                int length = mac.Length;
                mac = length > 24 ? mac.Substring(0, 24) : mac + baseString.Substring(0,24 - length);
                byte[] KEY = Encoding.ASCII.GetBytes(mac);
                //byte[] KEY = Encoding.ASCII.GetBytes("xEu1p;LP7iZmxKyTtA*HV9pS");
                byte[] IV = Encoding.ASCII.GetBytes("j0?2UK+p");
                byte[] data = Encoding.Default.GetBytes(desString);
                byte[] des3 = MEDAS.Des3.Des3EncodeCBC(KEY, IV, data);
                string EncryptStr = Convert.ToBase64String(des3);
                return EncryptStr;
            }
            catch(Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("软件安装验证加密异常", ex.Message+ex.StackTrace);
                return "";
            }
        }
        void DeleteApplication()
        {
            string vBatFile = Path.GetDirectoryName(Application.ExecutablePath) + "\\Zswang.bat";
            using (StreamWriter vStreamWriter = new StreamWriter(vBatFile, false, Encoding.Default))
            {

                vStreamWriter.Write(string.Format(
                ":del\r\n" +
                " del \"{0}\"\r\n" +
                "if exist \"{0}\" goto del\r\n" + //此处已修改
                "del %0\r\n", Application.ExecutablePath));
            }
            //************ 执行批处理
            WinExec(vBatFile, 0);
            //************ 结束退出
            Close();
        }
    }
}
