using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace MEDAS
{
    partial class MEDA_WindowsServer : ServiceBase
    {
        MainClass mc = new MainClass();

        public MEDA_WindowsServer()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO:  在此处添加代码以启动服务。
            mc.Start();
        }

        protected override void OnStop()
        {
            // TODO:  在此处添加代码以执行停止服务所需的关闭操作。
            mc.Stop();
        }
    }
}
