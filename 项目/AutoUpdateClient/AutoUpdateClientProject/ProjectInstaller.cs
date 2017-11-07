using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdateClient
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void ProjectInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName == "XDTCUpdateServer")
                    {
                        service.Start();
                        service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 30));
                        ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "服务已经启动", "");
                    }
                }
            }
            catch (Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "服务已启动异常", ex.Message);
            }
        }
    }
}
