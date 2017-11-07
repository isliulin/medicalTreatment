using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MEDAS
{
    public class ULT_3000Plus
    {
        public static void ProtocolAnalysis(byte[] frame, string ComName)
        {
            try
            {
                if (frame[0] == 0x02)//起始位
                {
                    //包的总长度
                    int frameLength = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[1], frame[2]), 16);

                    #region Message 域
                    ULT_3000PlusMessage u3 = new ULT_3000PlusMessage();
                    //data长度
                    u3.Length = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[3], frame[4]), 16);
                    u3.Type = frame[5];
                    u3.DataType = frame[6];

                    //20个参数
                    u3.WBC = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[7], frame[8]), 16);
                    u3.LYPercentage = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[9], frame[10]), 16);
                    u3.MOPercentage = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[11], frame[12]), 16);
                    u3.GRPercentage = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[13], frame[14]), 16);
                    u3.LYValue = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[15], frame[16]), 16);
                    u3.MOValue = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[17], frame[18]), 16);
                    u3.GRValue = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[19], frame[20]), 16);
                    u3.RBC = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[21], frame[22]), 16);
                    u3.HCT = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[23], frame[24]), 16);
                    u3.MCV = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[25], frame[26]), 16);
                    u3.MCH = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[27], frame[28]), 16);
                    u3.MCHC = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[29], frame[30]), 16);
                    u3.RDW_cv = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[31], frame[32]), 16);
                    u3.RDW_sd = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[33], frame[34]), 16);
                    u3.PLT = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[35], frame[36]), 16);
                    u3.MPV = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[37], frame[38]), 16);
                    u3.PDW = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[39], frame[40]), 16);
                    u3.PCT = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[41], frame[42]), 16);
                    u3.P_LCR = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[43], frame[44]), 16);
                    u3.P_LCC = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[45], frame[46]), 16);

                    //直方图
                    u3.WBCHistogram = new byte[256];
                    Array.Copy(frame, 47, u3.WBCHistogram, 0, 256);
                    u3.RBCHistogram = new byte[256];
                    Array.Copy(frame, 303, u3.RBCHistogram, 0, 256);
                    u3.PLTHistogram = new byte[128];
                    Array.Copy(frame, 559, u3.PLTHistogram, 0, 128);

                    //测量标杆
                    u3.WBC_SurveyorPole = Convert.ToUInt32(string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", frame[687], frame[688], frame[689], frame[690]), 16);
                    u3.RBC_SurveyorPole = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[691], frame[692]), 16);
                    u3.PLT_SurveyorPole = Convert.ToUInt16(string.Format("{0:X2}{1:X2}", frame[693], frame[694]), 16);

                    //报警值
                    u3.R1_Alarm = frame[695];
                    u3.R2_Alarm = frame[696];
                    u3.R3_Alarm = frame[697];
                    u3.R4_Alarm = frame[698];
                    u3.PM_Alarm = frame[699];

                    //样品编号
                    byte[] sn = new byte[6];   //设备编号
                    Array.Copy(frame, 700, sn, 0, 6);
                    u3.SampleNo = Encoding.ASCII.GetString(sn);  //获取设备编号ASCII

                    //日期时间
                    try
                    {
                        string dt = "";
                        for (int i = 0; i < 6; i++)
                            dt += TimeFormat(frame[706 + i]);
                        u3.DateTime_SampleNo = DateTime.ParseExact(dt, "yyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                        u3.DateTime_SampleNo = DateTime.Now;
                    }
                    try
                    {
                        string Json = Newtonsoft.Json.JsonConvert.SerializeObject(u3);
                        DataUpload.ReportAsyn.BeginInvoke(Json, MainStatic.GetDeviceSn(ComName), null, null);
                    }
                    catch (Exception ex)
                    {
                        ToolAPI.XMLOperation.WriteLogXmlNoTail("数据将要转JSON异常", ex.Message);
                    }
                    #endregion

                    //结束符
                    //校验位
                }
            }
            catch(Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("ULT_3000Plus.ProtocolAnalysis数据解析出现异常：", ex.Message);
            }
        }
        public static string TimeFormat(byte value)
        {
            string valueTemp = value.ToString();
            if (valueTemp.Length > 2)
                return valueTemp.Substring(valueTemp.Length - 2, 2);
            else if (valueTemp.Length == 1)
                return "0" + valueTemp;
            else return valueTemp;
        }
    
        
    }

   

}
