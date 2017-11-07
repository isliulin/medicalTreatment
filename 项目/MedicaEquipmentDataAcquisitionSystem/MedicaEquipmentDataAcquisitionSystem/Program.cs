using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MEDAS
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
                Application.Run(new winShow());
            }
            else//为服务
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                { 
                     new MEDA_WindowsServer() 
                 };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
