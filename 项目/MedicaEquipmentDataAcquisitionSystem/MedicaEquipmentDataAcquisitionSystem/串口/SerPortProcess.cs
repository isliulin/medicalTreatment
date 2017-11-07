using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDAS
{
    /// <summary>
    /// 基础串口类
    /// </summary>
    public class SerPortProcess
    {
        #region 字段和属性
        public byte[] sendData = new byte[1] { 0x06 };


        public SerialPort ComPort { get; set; }
        /// <summary>
        /// 串口接收事件
        /// </summary>
        public SerialDataReceivedEventHandler Receive { get; set; }
        /// <summary>
        /// 串口名称
        /// </summary>
        public string PortName { get; set; }
        /// <summary>
        /// 波特率
        /// </summary>
        public string BaudRate { get; set; }
        /// <summary>
        /// 奇偶校验
        /// </summary>
        public Parity Parity { get; set; }
        /// <summary>
        /// 数据位
        /// </summary>
        public string DataBits { get; set; }
        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits { get; set; }

        /// <summary>
        /// 串口接收事件后的异步执行方法的委托
        /// </summary>
        public Action<byte[],string> DataAnalysisAction;
        public Action<byte[],string> DataPutTogetherAction;
        List<byte> DataTemp = new List<byte>();//组合日志的数组
        bool isStart = false;
        bool isBC_1800 = false;
        #endregion

        #region 构造方法
        /// <summary>
        /// 读文件进行的初始化
        /// </summary>
        /// <param name="Receive">串口接受事件</param>
        public SerPortProcess(string Name, Action<byte[], string> DataAnalysisActionTemp)
        {
            try
            {
                PortName = Name;
                BaudRate = "9600";
                this.Parity = Parity.None;
                DataBits = "8";
                this.StopBits = StopBits.One;
                this.Receive = ComReceive;
                DataPutTogetherAction = DataPutTogether;
                ComPort = new SerialPort();
                ComPort.PortName = PortName;//设置要打开的串口
                ComPort.BaudRate = int.Parse(BaudRate);//设置当前波特率
                ComPort.Parity = Parity;//设置当前校验位
                ComPort.DataBits = int.Parse(DataBits);//设置当前数据位
                ComPort.StopBits = StopBits;//设置当前停止位  
                ComPort.DataReceived += Receive; //绑定串口接收事件
                DataAnalysisAction = DataAnalysisActionTemp;
            }
            catch (Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("串口构造异常：", ex.Message);
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取串口列表
        /// </summary>
        /// <returns>返回串口的字符串数组</returns>
        static public string[] GetLocalPortNames()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        /// 初始化并打开串口
        /// </summary>
        /// <param name="isEnableRts">否启用请求发送 (RTS) 信号</param>
        /// <returns></returns>
        public bool OpenSerPort()
        {
            try//尝试打开串口
            {
                try
                {
                    if (!ComPort.IsOpen)
                        ComPort.Open();//打开串口
                    bool result = ComPort.IsOpen;
                    //if (result) DataUpload.MonitorAsyn.BeginInvoke("0", MainStatic.GetDeviceSn(ComPort.PortName), null, null);
                    //else DataUpload.MonitorAsyn.BeginInvoke("10001", MainStatic.GetDeviceSn(ComPort.PortName), null, null);
                    ToolAPI.XMLOperation.WriteLogXmlNoTail("串口打开：", PortName + ";" + BaudRate + ";" + result.ToString());
                }
                catch (Exception ex) { ToolAPI.XMLOperation.WriteLogXmlNoTail("串口打开异常：", ex.Message); }
                return true;
            }
            catch (Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("串口打开异常：", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        public bool CloseSerPort()
        {
            try//尝试关闭串口
            {
                ComPort.DiscardOutBuffer();//清发送缓存
                ComPort.DiscardInBuffer();//清接收缓存
                ComPort.Close();//关闭串口
                bool result = ComPort.IsOpen;
                ToolAPI.XMLOperation.WriteLogXmlNoTail("串口关闭：", PortName + ";" + BaudRate + ";" + result.ToString());
                //if (result) DataUpload.MonitorAsyn.BeginInvoke("10001", MainStatic.GetDeviceSn(ComPort.PortName), null, null);
                //else DataUpload.MonitorAsyn.BeginInvoke("0", MainStatic.GetDeviceSn(ComPort.PortName), null, null);
                return true;
            }
            catch (Exception ex)//如果在未关闭串口前，串口就已丢失，这时关闭串口会出现异常
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("串口关闭异常：", ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="dataByte"></param>
        public bool SendData(SerialPort serport, byte[] dataByte)
        {
            try
            {
                serport.Write(dataByte, 0, dataByte.Length);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 串口接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComReceive(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort seriaPortTemp = sender as SerialPort;
            byte[] recBuffer;//接收缓冲区
            try
            {
                recBuffer = new byte[seriaPortTemp.BytesToRead];//接收数据缓存大小
                seriaPortTemp.Read(recBuffer, 0, recBuffer.Length);//读取数据
                if (recBuffer.Length > 0 && recBuffer[0] == 0x10)
                {
                    seriaPortTemp.Write(sendData, 0, 1);
                    ToolAPI.XMLOperation.WriteLogXmlNoTail("串口开始发送数据：", "");
                    isStart = true;
                    isBC_1800 = true;
                    if (recBuffer.Length>1)
                    {
                        byte[] butterTemp = new byte[recBuffer.Length-1];
                        Array.Copy(recBuffer, 1, butterTemp, 0, recBuffer.Length - 1);
                        DataPutTogetherAction.BeginInvoke(butterTemp, seriaPortTemp.PortName, null, null);
                        ToolAPI.XMLOperation.WriteLogXmlNoTail("BC1800开始携带的", butterTemp.Length.ToString());
                    }
                      
                }
                else if (recBuffer.Length > 0 && recBuffer[0] == 0x0f)
                {
                    seriaPortTemp.Write(sendData, 0, 1);
                    ToolAPI.XMLOperation.WriteLogXmlNoTail("串口停止发送数据：", "");
                    isStart = false;
                    isBC_1800 = true;
                    DataPutTogetherAction.BeginInvoke(recBuffer,seriaPortTemp.PortName, null, null);
                }
                else if (recBuffer.Length > 0 && (recBuffer[0] == 0x77 || recBuffer[0] == 0x49))
                {
                    isBC_1800 = false;
                    DataPutTogetherAction.BeginInvoke(recBuffer, seriaPortTemp.PortName, null, null);
                }
                else if (recBuffer.Length > 0 && (recBuffer[0] == 0x42 ))
                {
                    isBC_1800 = false;
                    DataPutTogetherAction.BeginInvoke(recBuffer, seriaPortTemp.PortName, null, null);
                }
                else
                {
                     DataPutTogetherAction.BeginInvoke(recBuffer, seriaPortTemp.PortName, null, null);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                ToolAPI.XMLOperation.WriteLogXmlNoTail("串口接收事件异常：", ex.Message);
            }
        }

        void DataPutTogether(byte[] dataTemp,string ComName)
        {
            if (isBC_1800)
            {
                if (isStart)
                {
                    if (dataTemp != null && dataTemp.Length > 0)
                        DataTemp.AddRange(dataTemp);
                }
                else
                {
                    byte[] byteTemp = new byte[DataTemp.Count];
                    DataTemp.CopyTo(byteTemp);
                    DataAnalysisAction.BeginInvoke(byteTemp, ComName, null, null);//异步执行
                    ToolAPI.XMLOperation.WriteLogXmlNoTail("BC1800开始结束后DataTemp长", DataTemp.Count.ToString());
                    DataTemp.Clear();
                    isBC_1800 = false;
                  
                }
            }
            else
            {
                if (dataTemp != null && dataTemp.Length > 0)
                    DataTemp.AddRange(dataTemp);
                //无法判断结束只能这样了  针对那个bm12设备做的
                try
                {
                    if (DataTemp[DataTemp.Count - 1] == 0x0D && DataTemp[DataTemp.Count - 2] == 0x0A && DataTemp[DataTemp.Count - 3] == 0x3A && DataTemp[DataTemp.Count - 4] == 0x45)
                    {
                        byte[] byteTemp = new byte[DataTemp.Count];
                        DataTemp.CopyTo(byteTemp);
                        DataAnalysisAction.BeginInvoke(byteTemp, ComName, null, null);//异步执行
                        ToolAPI.XMLOperation.WriteLogXmlNoTail("bm21开始结束后DataTemp长", DataTemp.Count.ToString());
                        DataTemp.Clear();
                       
                    }
                    else if (DataTemp[DataTemp.Count - 1] == 0x0D && DataTemp[DataTemp.Count - 2] == 0x0A && DataTemp[DataTemp.Count - 3] == 0x2E)
                    {
                        byte[] byteTemp = new byte[DataTemp.Count];
                        DataTemp.CopyTo(byteTemp);
                        DataAnalysisAction.BeginInvoke(byteTemp, ComName, null, null);//异步执行
                        ToolAPI.XMLOperation.WriteLogXmlNoTail("bm200开始结束后DataTemp长", DataTemp.Count.ToString());
                        DataTemp.Clear();
                        
                    }
                }
                catch (Exception)
                { }
            }
        }

        #endregion
    }
}
