using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MEDAS
{
    public class MainClass
    {
        public void Start()
        {
            try
            {
                string result = DataUpload.VerifyAsyn();
                if (result.Contains("\"code\":200"))
                {
                    ToolAPI.XMLOperation.WriteLogXmlNoTail("身份验证通过：", result);
                    //开启串口
                    SerPortList.DataAnalysisAction = ProtocolAnalysis.DataAnalysis;
                    SerPortList.Start();
                }
                else
                {
                    ToolAPI.XMLOperation.WriteLogXmlNoTail("身份验证未通过：", result);
                    //测试用
                    //SerPortList.DataAnalysisAction = ProtocolAnalysis.DataAnalysis;
                    //SerPortList.Start();
                }
            }
            catch (Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("软件启动发生异常：", ex.Message + ex.StackTrace);
            }

        }

        public void Stop()
        {
            try
            {
                SerPortList.Stop();
                ToolAPI.XMLOperation.WriteLogXmlNoTail("程序关闭", "");
            }
            catch (Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("软件关闭发生异常：", ex.Message + ex.StackTrace);
            }
        }
    }
}
