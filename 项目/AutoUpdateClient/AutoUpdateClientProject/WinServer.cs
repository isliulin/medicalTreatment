using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdateClient
{
    public class WinServer
    {
        /// <summary>  
        /// 判断是否安装了某个服务  
        /// </summary>  
        /// <param name="serviceName"></param>  
        /// <returns></returns>  
        public static bool ISWindowsServiceInstalled(string serviceName)
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();


                foreach (ServiceController service in services)
                {
                    if (service.ServiceName == serviceName)
                    {
                        return true;
                    }
                }


                return false;
            }
            catch
            { return false; }
        }

        /// <summary>  
        /// 启动某个服务  
        /// </summary>  
        /// <param name="serviceName"></param>  
        public static bool StartService(string serviceName)
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();


                foreach (ServiceController service in services)
                {
                    if (service.ServiceName == serviceName)
                    {
                        if ((service.Status == ServiceControllerStatus.Stopped)
                         || (service.Status == ServiceControllerStatus.StopPending))
                        service.Start();
                        service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 30));
                        ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "服务已经启动", serviceName);
                        return true;
                    }
                }
                return false;
            }
            catch(Exception ex){
                ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "服务启动出现异常", ex.Message);
                return false;
            }
        }
        /// <summary>  
        /// 停止某个服务  
        /// </summary>  
        /// <param name="serviceName"></param>  
        public static bool StopService(string serviceName)
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName == serviceName)
                    {
                        try
                        {
                            if ((service.Status == ServiceControllerStatus.Running))
                            service.Stop();
                        }
                        catch (Exception) { }
                        //Thread.Sleep(10000);
                        service.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));
                        ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "服务已经停止", "");
                        //return !ISStart(serviceName);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex){
                ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "服务停止出现异常", ex.Message);
                return false;
            }
        }

        /// <summary>  
        /// 判断某个服务是否启动  
        /// </summary>  
        /// <param name="serviceName"></param>  
        public static bool ISStart(string serviceName)
        {
            bool result = true;


            try
            {
                ServiceController[] services = ServiceController.GetServices();

                foreach (ServiceController service in services)
                {
                    if (service.ServiceName == serviceName)
                    {
                        if ((service.Status == ServiceControllerStatus.Stopped)
                            || (service.Status == ServiceControllerStatus.StopPending))
                        {
                            result = false;
                        }
                    }
                }
            }
            catch { }


            return result;
        }  
    }
}
