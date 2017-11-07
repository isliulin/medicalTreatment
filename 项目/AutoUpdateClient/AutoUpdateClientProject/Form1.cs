using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdateClient
{
    public partial class Form1 : Form
    {
        MainClass mc = new MainClass();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mc.Start();
            //using (
            //    ServiceReference1.Service1Client sc = new ServiceReference1.Service1Client())
            //{
            //    sc.Open();
            //    //MessageBox.Show(sc.GetData(10));

            //    bool istest = sc.IsUpdate();
            //    string[] listTest = sc.GetFileList();
            //    foreach(string str in listTest)
            //    {
            //        Stream sourceStream = sc.DownLoadFile(str);
            //        if (sourceStream != null)
            //        {
            //            if (sourceStream.CanRead)
            //            {
            //                using (FileStream fs = new FileStream(@"J:\clientTest\" + str, FileMode.Create, FileAccess.Write, FileShare.None))
            //                {
            //                    const int bufferLength = 4096;
            //                    byte[] myBuffer = new byte[bufferLength];
            //                    int count;
            //                    while ((count = sourceStream.Read(myBuffer, 0, bufferLength)) > 0)
            //                    {
            //                        fs.Write(myBuffer, 0, count);
            //                    }
            //                    fs.Close();
            //                    sourceStream.Close();
            //                }
            //            }
            //        }
            //    }
            //    sc.Close();
            //}  
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mc.Stop();
            //bool isInstalled = WinServer.ISWindowsServiceInstalled("XunDaTianChneg");
            //bool isopen = WinServer.ISStart("XunDaTianChneg");
            //if (isopen)
            //{
            //    WinServer.StopService("XunDaTianChneg");
            //}
            //else
            //{
            //    WinServer.StartService("XunDaTianChneg");
            //}
        }
    }
}
