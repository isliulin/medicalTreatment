using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviceInit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            try{
            string devicesnList = ToolAPI.INIOperate.IniReadValue("Base", "devicesnList", Application.StartupPath + "\\Config.ini");
            if (!string.IsNullOrEmpty(devicesnList))
            {
                string[] devicesnListTrem = devicesnList.Split(',');
                for(int i=0;i<9;i++)
                {
                    string[] tremAry = devicesnListTrem[i].Split('&');
                    switch(i+1)
                    {
                        case 1: device1.Text = tremAry[1] ?? ""; des1.Text = tremAry[2] ?? ""; break;
                        case 2: device2.Text = tremAry[1] ?? ""; des2.Text = tremAry[2] ?? ""; break;
                        case 3: device3.Text = tremAry[1] ?? ""; des3.Text = tremAry[2] ?? ""; break;
                        case 4: device4.Text = tremAry[1] ?? ""; des4.Text = tremAry[2] ?? ""; break;
                        case 5: device5.Text = tremAry[1] ?? ""; des5.Text = tremAry[2] ?? ""; break;
                        case 6: device6.Text = tremAry[1] ?? ""; des6.Text = tremAry[2] ?? ""; break;
                        case 7: device7.Text = tremAry[1] ?? ""; des7.Text = tremAry[2] ?? ""; break;
                        case 8: device8.Text = tremAry[1] ?? ""; des8.Text = tremAry[2] ?? ""; break;
                        case 9: device9.Text = tremAry[1] ?? ""; des9.Text = tremAry[2] ?? ""; break;
                    }
                }
            }
            }
            catch(Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\DeviceInit\", "设备列表加载异常", ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder devicesnList = new StringBuilder();
            devicesnList.Append("COM1").Append("&").Append(device1.Text).Append("&").Append(des1.Text).Append("&,");
            devicesnList.Append("COM2").Append("&").Append(device2.Text).Append("&").Append(des2.Text).Append("&,");
            devicesnList.Append("COM3").Append("&").Append(device3.Text).Append("&").Append(des3.Text).Append("&,");
            devicesnList.Append("COM4").Append("&").Append(device4.Text).Append("&").Append(des4.Text).Append("&,");
            devicesnList.Append("COM5").Append("&").Append(device5.Text).Append("&").Append(des5.Text).Append("&,");
            devicesnList.Append("COM6").Append("&").Append(device6.Text).Append("&").Append(des6.Text).Append("&,");
            devicesnList.Append("COM7").Append("&").Append(device7.Text).Append("&").Append(des7.Text).Append("&,");
            devicesnList.Append("COM8").Append("&").Append(device8.Text).Append("&").Append(des8.Text).Append("&,");
            devicesnList.Append("COM9").Append("&").Append(device9.Text).Append("&").Append(des9.Text).Append("&");
            ToolAPI.INIOperate.IniWriteValue("Base", "devicesnList", devicesnList.ToString(), Application.StartupPath + "\\Config.ini");
            ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\DeviceInit\", "设备列表更新", devicesnList.ToString());
            MessageBox.Show("保存成功");
        }
    }
}
