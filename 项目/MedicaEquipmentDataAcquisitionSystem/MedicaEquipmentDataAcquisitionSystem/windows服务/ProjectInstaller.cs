using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;

namespace MEDAS
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            using (SettingHelper settingHelper = new SettingHelper())
            {
                serviceInstaller1.ServiceName = settingHelper.ServiceName;
                serviceInstaller1.DisplayName = settingHelper.DisplayName;
                serviceInstaller1.Description = settingHelper.Description;
            }
        }

        //安装后自动启动
        private void ProjectInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            //Process p = new Process();
            //p.StartInfo.FileName = "cmd.exe";
            //p.StartInfo.UseShellExecute = false;
            //p.StartInfo.RedirectStandardInput = true;
            //p.StartInfo.RedirectStandardOutput = true;
            //p.StartInfo.RedirectStandardError = true;
            //p.StartInfo.CreateNoWindow = true;
            //p.Start();
            ////string Cmdstring = "sc start myservice"; //CMD命令
            //string Cmdstring = "sc start " + serviceInstaller1.ServiceName; //CMD命令; //CMD命令
            //p.StandardInput.WriteLine(Cmdstring);
            //p.StandardInput.WriteLine("exit");
            //ToolAPI.XMLOperation.WriteLogXmlNoTail("服务已经启动", "");
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName == serviceInstaller1.ServiceName)
                    {
                        service.Start();
                        service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 30));
                        ToolAPI.XMLOperation.WriteLogXmlNoTail("服务已经启动", "");
                    }
                }
            }
            catch (Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("服务已启动异常", ex.Message);
            }
        }
    }
}
