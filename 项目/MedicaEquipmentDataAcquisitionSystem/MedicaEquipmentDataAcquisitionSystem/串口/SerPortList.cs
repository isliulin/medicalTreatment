using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MEDAS
{
    public static class SerPortList
    {
        static Thread UpdateSerPortDicThread, DeviceInitThread;
        static Dictionary<string, SerPortProcess> SerPortDic = new Dictionary<string, SerPortProcess>();
        static public Action<byte[], string> DataAnalysisAction;
        static void UpdateSerPortDic()
        {
            while (true)
            {
                try
                {
                    string[] serPortAry = SerPortProcess.GetLocalPortNames();//得到串口列表
                    string serPortnameStr = "";
                    //对获得的串口组进行遍历并更新SerPortDic以及对应值的串口的状态为开启
                    if (serPortAry.Length > 0)
                        foreach (string serPortname in serPortAry)
                        {
                            serPortnameStr += serPortname + ";";
                            if (SerPortDic.Keys.Contains(serPortname))
                            {
                                if (!(SerPortDic[serPortname].ComPort.IsOpen))
                                    SerPortDic[serPortname].OpenSerPort();
                            }
                            else
                            {
                                SerPortProcess serPortTemp = new SerPortProcess(serPortname, DataAnalysisAction);
                                serPortTemp.OpenSerPort();
                                SerPortDic.Add(serPortname, serPortTemp);
                            }
                        }
                    //删除不存在的串口
                    String[] keyArr = SerPortDic.Keys.ToArray<String>();
                    for (int i = 0; i < keyArr.Length; i++)
                    {
                        if (!serPortnameStr.Contains(keyArr[i]))
                        {
                            SerPortDic[keyArr[i]].CloseSerPort();
                            SerPortDic.Remove(keyArr[i]);
                        }
                    }
                    ToolAPI.XMLOperation.WriteLogXmlNoTail("UpdateSerPortDic的遍历结束", serPortnameStr + ";" + SerPortDic.Count.ToString());

                }
                catch (Exception ex)
                {
                    ToolAPI.XMLOperation.WriteLogXmlNoTail("UpdateSerPortDic的遍历出现异常", ex.Message + ";" + ex.StackTrace);
                }
                try
                {
                    Dictionary<string, string> decTemp = MainStatic.Clone_devicesnListDic();
                    if (decTemp != null && decTemp.Count > 0)
                    {
                        foreach (var dec in decTemp)
                        {
                            bool isState = false;
                            if (SerPortDic.ContainsKey(dec.Key))
                            {
                                if (SerPortDic[dec.Key].ComPort != null)
                                {
                                    if (SerPortDic[dec.Key].ComPort.IsOpen)
                                        isState = true;
                                    else
                                        isState = false;
                                }
                                else
                                    isState = false;

                            }
                            else
                                isState = false;
                            if (isState)
                            {
                                if (!string.IsNullOrEmpty(dec.Value))
                                {
                                    DataUpload.MonitorAsyn.BeginInvoke("0", dec.Value, null, null);
                                    ToolAPI.XMLOperation.WriteLogXmlNoTail("心跳发送", "0" + ";" + dec.Key + ";" + dec.Value);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(dec.Value))
                                {
                                    DataUpload.MonitorAsyn.BeginInvoke("10001", dec.Value, null, null);
                                    ToolAPI.XMLOperation.WriteLogXmlNoTail("心跳发送", "10001" + ";" + dec.Key + ";" + dec.Value);
                                }
                            }

                        }
                    }
                }
                catch (Exception exa)
                {
                    ToolAPI.XMLOperation.WriteLogXmlNoTail("心跳异常", exa.Message + ";" + exa.StackTrace);
                }
                Thread.Sleep(600000);
            }

        }

        static void DeviceInit()
        {
            while (true)
            {
                try
                {

                    string devicesnList = ToolAPI.INIOperate.IniReadValue("Base", "devicesnList", Application.StartupPath + "\\Config.ini");
                    string[] devicesnListTrem = devicesnList.Split(',');
                    if (devicesnListTrem.Length > 0)
                        foreach (string trem in devicesnListTrem)
                        {
                            string[] tremAry = trem.Split('&');
                            if (tremAry.Length >= 2)
                            {
                                bool result = MainStatic.Update_DeviceSn(tremAry[0], tremAry[1]);
                                ////if (result)
                                ////{
                                ////    string state = "";
                                ////    if (SerPortDic.Keys.Contains(tremAry[0]))
                                ////    {
                                ////        if ((SerPortDic[tremAry[0]].ComPort.IsOpen))
                                ////            state = "0";
                                ////        else
                                ////            state = "10001";
                                ////    }
                                ////    else
                                ////    {
                                ////        state = "10001";
                                ////    }
                                ////    DataUpload.MonitorAsyn.BeginInvoke(state, MainStatic.GetDeviceSn(tremAry[0]), null, null);
                                //}
                            }
                            //devicesnListDic.Add(tremAry[0], tremAry[1]);
                        }
                    ToolAPI.XMLOperation.WriteLogXmlNoTail("DeviceInit的初始化", devicesnList);
                }
                catch (Exception ex)
                {
                    ToolAPI.XMLOperation.WriteLogXmlNoTail("DeviceInit的初始化出现异常", ex.Message + ";" + ex.StackTrace);
                }
                Thread.Sleep(60000);
            }

        }

        static public void Start()
        {
            UpdateSerPortDicThread = new Thread(UpdateSerPortDic) { IsBackground = true };
            UpdateSerPortDicThread.Start();

            DeviceInitThread = new Thread(DeviceInit) { IsBackground = true };
            DeviceInitThread.Start();
        }

        static public void Stop()
        {
            try
            {
                if (UpdateSerPortDicThread != null)
                {
                    UpdateSerPortDicThread.Abort();
                    UpdateSerPortDicThread.Join();
                }
                UpdateSerPortDicThread = null;
                if (SerPortDic.Count > 0)
                {
                    foreach (var keyValue in SerPortDic)
                    {
                        keyValue.Value.CloseSerPort();//关闭每一个串口
                    }
                }
                SerPortDic.Clear();
            }
            catch (Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("SerPortList的Stop出现异常", ex.Message + ";" + ex.StackTrace);
            }
        }

    }
}
