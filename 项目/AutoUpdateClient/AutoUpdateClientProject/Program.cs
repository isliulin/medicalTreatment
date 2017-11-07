using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdateClient
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (false)//为窗体
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else//为服务
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                { 
                     new UpdateServer() 
                 };
                ServiceBase.Run(ServicesToRun);
            }

        }
    }
}
