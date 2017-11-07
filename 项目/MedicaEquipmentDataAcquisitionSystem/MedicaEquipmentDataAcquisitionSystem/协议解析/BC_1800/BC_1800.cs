 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolAPI;

namespace MEDAS
{
    public class BC_1800
    {
        //拆MD
        public static void ProtocolAnalysis(byte[] frame,string ComName)
        {
            try
            {
                string dataHexString = ConvertData.ToHexString_Space(frame, 0, frame.Length);
                //Message 拆包，防止连包
                string[] MessageFrame = CutOutStringByString(dataHexString, new string[] { "53 65 70 4C 69 6E 65" });
                foreach (string Message in MessageFrame)//针对每一个Message进行的解析
                {
                    try
                    {
                        int index = Message.LastIndexOf("05 ") + "05 ".Length - 1;
                        int index_s = Message.LastIndexOf(" 03");
                        string domain = Message.Substring(index + 1, index_s - index - 1);
                        string domainVal = Message.Substring(index_s+4);
                        string MD =HexStringToAsciiString(domain.Replace(" ",""));
                        switch (MD)
                        {
                            case "CTR": SampleResultAnalysis(domainVal, ComName); break;//CTR 正常结果数据包
                            case "QCR": StandardQualityControlAnalysis(domainVal, ComName); break;//CTR 正常结果数据包
                            case "QCC": RunQualityControlAnalysis(domainVal, ComName); break;//CTR 正常结果数据包
                            default: break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ToolAPI.XMLOperation.WriteLogXmlNoTail("BC_1800.ProtocolAnalysis数据解析出现异常：", ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("BC_1800.ProtocolAnalysis外层数据解析出现异常：", ee.Message);
            }
        }

        #region 正常计数结果数据包
        private static void SampleResultAnalysis(string Segment,string ComName)
        {
            SampleResult sr = new SampleResult();
            string[] SegmentFrame = CutOutStringByString(Segment, new string[] { "04" });//得到SD数组 SE
            foreach (string SegmentObj in SegmentFrame)//针对每一个Message进行的解析
            {
                try
                {
                    string[] SegmentTemp = CutOutStringByString(SegmentObj, new string[] { "0C" });//特殊分隔符SD与FD
                    if (SegmentTemp.Length == 2)
                    {
                        string SD = HexStringToAsciiString(SegmentTemp[0].Replace(" ",""));
                        string[] FieldTemp = CutOutStringByString(SegmentTemp[1], new string[] { "08" });//FE
                        switch (SD)
                        {
                            #region 分支运算
                            case "SampleInfo":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "SampleID": sr.SampleInfo_SampleID = V; break;
                                            case "Mode": sr.SampleInfo_Mode = V; break;
                                            case "TestTime": sr.SampleInfo_TestTime = V; break;
                                            case "Name": sr.SampleInfo_Name = V; break;
                                            case "Gender": sr.SampleInfo_Gender = V; break;
                                            case "AgeVal": sr.SampleInfo_AgeVal = V; break;
                                            case "AgeType": sr.SampleInfo_AgeType = V; break;
                                            case "Dept": sr.SampleInfo_Dept = V; break;
                                            case "ChartNo": sr.SampleInfo_ChartNo = V; break;
                                            case "BedNo": sr.SampleInfo_BedNo = V; break;
                                            case "Sender": sr.SampleInfo_Sender = V; break;
                                            case "Tester": sr.SampleInfo_Tester = V; break;
                                            case "Checker": sr.SampleInfo_Checker = V; break;
                                            case "Group": sr.SampleInfo_Group = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "WBC":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": sr.WBC_Val = V; break;
                                            case "Low": sr.WBC_Low = V; break;
                                            case "High": sr.WBC_High = V; break;
                                            case "Unit": sr.WBC_Unit = V; break;
                                            case "Flag": sr.WBC_Flag = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "Lymph#":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": sr.LymphValue_Val = V; break;
                                            case "Low": sr.LymphValue_Low = V; break;
                                            case "High": sr.LymphValue_High = V; break;
                                            case "Unit": sr.LymphValue_Unit = V; break;
                                            case "Flag": sr.LymphValue_Flag = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "Mid#":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": sr.MidValue_Val = V; break;
                                            case "Low": sr.MidValue_Low = V; break;
                                            case "High": sr.MidValue_High = V; break;
                                            case "Unit": sr.MidValue_Unit = V; break;
                                            case "Flag": sr.MidValue_Flag = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "Gran#":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": sr.GranValue_Val = V; break;
                                            case "Low": sr.GranValue_Low = V; break;
                                            case "High": sr.GranValue_High = V; break;
                                            case "Unit": sr.GranValue_Unit = V; break;
                                            case "Flag": sr.GranValue_Flag = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "Lymph%":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": sr.LymphPercentage_Val = V; break;
                                            case "Low": sr.LymphPercentage_Low = V; break;
                                            case "High": sr.LymphPercentage_High = V; break;
                                            case "Unit": sr.LymphPercentage_Unit = V; break;
                                            case "Flag": sr.LymphPercentage_Flag = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "Mid%":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": sr.MidPercentage_Val = V; break;
                                            case "Low": sr.MidPercentage_Low = V; break;
                                            case "High": sr.MidPercentage_High = V; break;
                                            case "Unit": sr.MidPercentage_Unit = V; break;
                                            case "Flag": sr.MidPercentage_Flag = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "Gran%":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": sr.GranPercentage_Val = V; break;
                                            case "Low": sr.GranPercentage_Low = V; break;
                                            case "High": sr.GranPercentage_High = V; break;
                                            case "Unit": sr.GranPercentage_Unit = V; break;
                                            case "Flag": sr.GranPercentage_Flag = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "HGB":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": sr.HGB_Val = V; break;
                                            case "Low": sr.HGB_Low = V; break;
                                            case "High": sr.HGB_High = V; break;
                                            case "Unit": sr.HGB_Unit = V; break;
                                            case "Flag": sr.HGB_Flag = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "RBC":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": sr.RBC_Val = V; break;
                                            case "Low": sr.RBC_Low = V; break;
                                            case "High": sr.RBC_High = V; break;
                                            case "Unit": sr.RBC_Unit = V; break;
                                            case "Flag": sr.RBC_Flag = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "HCT":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": sr.HCT_Val = V; break;
                                            case "Low": sr.HCT_Low = V; break;
                                            case "High": sr.HCT_High = V; break;
                                            case "Unit": sr.HCT_Unit = V; break;
                                            case "Flag": sr.HCT_Flag = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "MCV":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": sr.MCV_Val = V; break;
                                            case "Low": sr.MCV_Low = V; break;
                                            case "High": sr.MCV_High = V; break;
                                            case "Unit": sr.MCV_Unit = V; break;
                                            case "Flag": sr.MCV_Flag = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "MCH":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": sr.MCH_Val = V; break;
                                            case "Low": sr.MCH_Low = V; break;
                                            case "High": sr.MCH_High = V; break;
                                            case "Unit": sr.MCH_Unit = V; break;
                                            case "Flag": sr.MCH_Flag = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "MCHC":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": sr.MCHC_Val = V; break;
                                            case "Low": sr.MCHC_Low = V; break;
                                            case "High": sr.MCHC_High = V; break;
                                            case "Unit": sr.MCHC_Unit = V; break;
                                            case "Flag": sr.MCHC_Flag = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "RDW-CV":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": sr.RDWCV_Val = V; break;
                                            case "Low": sr.RDWCV_Low = V; break;
                                            case "High": sr.RDWCV_High = V; break;
                                            case "Unit": sr.RDWCV_Unit = V; break;
                                            case "Flag": sr.RDWCV_Flag = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "RDW-SD":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": sr.RDWSD_Val = V; break;
                                            case "Low": sr.RDWSD_Low = V; break;
                                            case "High": sr.RDWSD_High = V; break;
                                            case "Unit": sr.RDWSD_Unit = V; break;
                                            case "Flag": sr.RDWSD_Flag = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "PLT":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": sr.PLT_Val = V; break;
                                            case "Low": sr.PLT_Low = V; break;
                                            case "High": sr.PLT_High = V; break;
                                            case "Unit": sr.PLT_Unit = V; break;
                                            case "Flag": sr.PLT_Flag = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "MPV":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": sr.MPV_Val = V; break;
                                            case "Low": sr.MPV_Low = V; break;
                                            case "High": sr.MPV_High = V; break;
                                            case "Unit": sr.MPV_Unit = V; break;
                                            case "Flag": sr.MPV_Flag = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "PDW":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": sr.PDW_Val = V; break;
                                            case "Low": sr.PDW_Low = V; break;
                                            case "High": sr.PDW_High = V; break;
                                            case "Unit": sr.PDW_Unit = V; break;
                                            case "Flag": sr.PDW_Flag = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "PCT":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": sr.PCT_Val = V; break;
                                            case "Low": sr.PCT_Low = V; break;
                                            case "High": sr.PCT_High = V; break;
                                            case "Unit": sr.PCT_Unit = V; break;
                                            case "Flag": sr.PCT_Flag = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "AlarmFlag":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Rm": sr.AlarmFlag_Rm = V; break;
                                            case "R1": sr.AlarmFlag_R1 = V; break;
                                            case "R2": sr.AlarmFlag_R2 = V; break;
                                            case "R3": sr.AlarmFlag_R3 = V; break;
                                            case "R4": sr.AlarmFlag_R4 = V; break;
                                            case "Pm": sr.AlarmFlag_Pm = V; break;
                                            case "Ps": sr.AlarmFlag_Ps = V; break;
                                            case "Pl": sr.AlarmFlag_P1 = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "SepLine":
                                #region
                                //foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                //{
                                //    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                //    if (SegmentTemp.Length == 2)
                                //    {
                                //        string FieldName = HexStringToAsciiString(VTemp[0]);
                                //        string V = HexStringToAsciiString(VTemp[1]);
                                //        switch (FieldName)
                                //        {
                                //            case "FD1": sr.SepLine_SepWBCLyLeft = V; break;
                                //            case "FD2": sr.SepLine_SepWBCLyMid = V; break;
                                //            case "FD3": sr.SepLine_SepWBCGranMid = V; break;
                                //            case "FD4": sr.SepLine_SepWBCGranRight = V; break;
                                //            case "FD5": sr.SepLine_SepRBCLef = V; break;
                                //            case "FD6": sr.SepLine_SepRBCRight = V; break;
                                //            case "FD7": sr.SepLine_SepPLTLeft = V; break;
                                //            case "FD8": sr.SepLine_SepPLTRight = V; break;
                                //            default: break;
                                //        }
                                //    }
                                //}
                                #endregion
                                break;
                            case "WBCHisto":
                                #region
                                //foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                //{
                                //    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                //    if (SegmentTemp.Length == 2)
                                //    {
                                //        string FieldName = HexStringToAsciiString(VTemp[0]);
                                //        switch (FieldName)
                                //        {
                                //            case "FD1": sr.WBCHisto_DataLen = HexStringToAsciiString(VTemp[1]); break;
                                //            case "FD2": sr.WBCHisto_MeteDataLen = HexStringToAsciiString(VTemp[1]); break;
                                //            case "FD3": sr.WBCHisto_WHistoData = ConvertData.HexToByte(VTemp[1]); break;
                                //            default: break;
                                //        }
                                //    }
                                //}
                                #endregion
                                break;
                            case "RBCHisto":
                                #region
                                //foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                //{
                                //    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                //    if (SegmentTemp.Length == 2)
                                //    {
                                //        string FieldName = HexStringToAsciiString(VTemp[0]);
                                //        switch (FieldName)
                                //        {
                                //            case "FD1": sr.RBCHisto_DataLen = HexStringToAsciiString(VTemp[1]); break;
                                //            case "FD2": sr.RBCHisto_MeteDataLen = HexStringToAsciiString(VTemp[1]); break;
                                //            case "FD3": sr.RBCHisto_RHistoData = ConvertData.HexToByte(VTemp[1]); break;
                                //            default: break;
                                //        }
                                //    }
                                //}
                                #endregion
                                break;
                            case "PLTHisto":
                                #region
                                //foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                //{
                                //    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                //    if (SegmentTemp.Length == 2)
                                //    {
                                //        string FieldName = HexStringToAsciiString(VTemp[0]);
                                //        switch (FieldName)
                                //        {
                                //            case "FD1": sr.PLTHisto_DataLen = HexStringToAsciiString(VTemp[1]); break;
                                //            case "FD2": sr.PLTHisto_MeteDataLen = HexStringToAsciiString(VTemp[1]); break;
                                //            case "FD3": sr.PLTHisto_PHistoData = ConvertData.HexToByte(VTemp[1]); break;
                                //            default: break;
                                //        }
                                //    }
                                //}
                                #endregion
                                break;
                            default: break;
                            #endregion
                        }
                    }
                }
                catch (Exception) { }

            }
            try
            {
                string Json = Newtonsoft.Json.JsonConvert.SerializeObject(sr);
                DataUpload.ReportAsyn.BeginInvoke(Json, MainStatic.GetDeviceSn(ComName), null, null);
            }
            catch(Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("数据将要转JSON异常", ex.Message);
            }
        }
        #endregion
        #region 标准质控
        private static void StandardQualityControlAnalysis(string Segment, string ComName)
        {
            StandardQualityControl sqc = new StandardQualityControl();
            string[] SegmentFrame = CutOutStringByString(Segment, new string[] { "04" });//得到SD数组 SE
            foreach (string SegmentObj in SegmentFrame)//针对每一个Message进行的解析
            {
                try
                {
                    string[] SegmentTemp = CutOutStringByString(SegmentObj, new string[] { "0C" });//特殊分隔符SD与FD
                    if (SegmentTemp.Length == 2)
                    {
                        string SD = HexStringToAsciiString(SegmentTemp[0].Replace(" ", ""));
                        string[] FieldTemp = CutOutStringByString(SegmentTemp[1], new string[] { "08" });//FE
                        switch (SD)
                        {
                            #region 分支运算
                            case "StQCInfo":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "FileNo": sqc.StQCInfo_FileNo = V; break;
                                            case "LotNo": sqc.StQCInfo_LotNo = V; break;
                                            case "ExpDate": sqc.StQCInfo_ExpDate = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "WBC":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ",""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Mean": sqc.WBC_Mean = V; break;
                                            case "Range": sqc.WBC_Range = V; break;
                                            case "Unit": sqc.WBC_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "RBC":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ",""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ",""));
                                        switch (FieldName)
                                        {
                                            case "Mean": sqc.RBC_Mean = V; break;
                                            case "Range": sqc.RBC_Range = V; break;
                                            case "Unit": sqc.RBC_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "HGB":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ",""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ",""));
                                        switch (FieldName)
                                        {
                                            case "Mean": sqc.HGB_Mean = V; break;
                                            case "Range": sqc.HGB_Range = V; break;
                                            case "Unit": sqc.HGB_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "PLT":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ",""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ",""));
                                        switch (FieldName)
                                        {
                                            case "Mean": sqc.PLT_Mean = V; break;
                                            case "Range": sqc.PLT_Range = V; break;
                                            case "Unit": sqc.PLT_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "Lymph# ":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ",""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ",""));
                                        switch (FieldName)
                                        {
                                            case "Mean": sqc.LymphValue_Mean = V; break;
                                            case "Range": sqc.LymphValue_Range = V; break;
                                            case "Unit": sqc.LymphValue_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "Lymph%":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ",""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ",""));
                                        switch (FieldName)
                                        {
                                            case "Mean": sqc.LymphPercentage_Mean = V; break;
                                            case "Range": sqc.LymphPercentage_Range = V; break;
                                            case "Unit": sqc.LymphPercentage_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "Gran#":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ",""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ",""));
                                        switch (FieldName)
                                        {
                                            case "Mean": sqc.GranValue_Mean = V; break;
                                            case "Range": sqc.GranValue_Range = V; break;
                                            case "Unit": sqc.GranValue_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "Gran%":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ",""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ",""));
                                        switch (FieldName)
                                        {
                                            case "Mean": sqc.GranPercentage_Mean = V; break;
                                            case "Range": sqc.GranPercentage_Range = V; break;
                                            case "Unit": sqc.GranhPercentage_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "HCT":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ",""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ",""));
                                        switch (FieldName)
                                        {
                                            case "Mean": sqc.HCT_Mean = V; break;
                                            case "Range": sqc.HCT_Range = V; break;
                                            case "Unit": sqc.HCT_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "MCV":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ",""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ",""));
                                        switch (FieldName)
                                        {
                                            case "Mean": sqc.MCV_Mean = V; break;
                                            case "Range": sqc.MCV_Range = V; break;
                                            case "Unit": sqc.MCV_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "MCH":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ",""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ",""));
                                        switch (FieldName)
                                        {
                                            case "Mean": sqc.MCH_Mean = V; break;
                                            case "Range": sqc.MCH_Range = V; break;
                                            case "Unit": sqc.MCH_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "MCHC ":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ",""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ",""));
                                        switch (FieldName)
                                        {
                                            case "Mean": sqc.MCHC_Mean = V; break;
                                            case "Range": sqc.MCHC_Range = V; break;
                                            case "Unit": sqc.MCHC_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            default: break;
                            #endregion
                        }
                    }
                }
                catch (Exception ) { }
            }
            try
            {
                string Json = Newtonsoft.Json.JsonConvert.SerializeObject(sqc);

                DataUpload.ReportAsyn.BeginInvoke(Json, MainStatic.GetDeviceSn(ComName), null, null);
            }
            catch (Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("数据将要转JSON异常", ex.Message);
            }
        }
        #endregion
        #region 运行质控
        private static void RunQualityControlAnalysis(string Segment, string ComName)
        {
            RunQualityControl rqc = new RunQualityControl();
            string[] SegmentFrame = CutOutStringByString(Segment, new string[] { "04" });//得到SD数组 SE
            foreach (string SegmentObj in SegmentFrame)//针对每一个Message进行的解析
            {
                try
                {
                    string[] SegmentTemp = CutOutStringByString(SegmentObj, new string[] { "0C" });//特殊分隔符SD与FD
                    if (SegmentTemp.Length == 2)
                    {
                        string SD = HexStringToAsciiString(SegmentTemp[0].Replace(" ", ""));
                        string[] FieldTemp = CutOutStringByString(SegmentTemp[1], new string[] { "08" });//FE
                        switch (SD)
                        {
                            #region 分支运算
                            case "RunQCInfo":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "FileNo": rqc.RunQCInfo_FileNo = V; break;
                                            case "LotNo": rqc.RunQCInfo_LotNo = V; break;
                                            case "ExpDate": rqc.RunQCInfo_ExpDate = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "WBC":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {
                                            case "Val": rqc.WBC_Val = V; break;
                                            case "Unit": rqc.WBC_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "RBC":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {

                                            case "Val": rqc.RBC_Val = V; break;
                                            case "Unit": rqc.RBC_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "HGB":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {

                                            case "Val": rqc.HGB_Val = V; break;
                                            case "Unit": rqc.HGB_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "PLT":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {

                                            case "Val": rqc.PLT_Val = V; break;
                                            case "Unit": rqc.PLT_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "Lymph# ":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {

                                            case "Val": rqc.LymphValue_Val = V; break;
                                            case "Unit": rqc.LymphValue_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "Lymph%":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {

                                            case "Val": rqc.LymphPercentage_Val = V; break;
                                            case "Unit": rqc.LymphPercentage_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "Gran#":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {

                                            case "Val": rqc.GranValue_Val = V; break;
                                            case "Unit": rqc.GranValue_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "Gran%":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {

                                            case "Val": rqc.GranPercentage_Val = V; break;
                                            case "Unit": rqc.GranhPercentage_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "HCT":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {

                                            case "Val": rqc.HCT_Val = V; break;
                                            case "Unit": rqc.HCT_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "MCV":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {

                                            case "Val": rqc.MCV_Val = V; break;
                                            case "Unit": rqc.MCV_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "MCH":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {

                                            case "Val": rqc.MCH_Val = V; break;
                                            case "Unit": rqc.MCH_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "MCHC ":
                                #region
                                foreach (string Field in FieldTemp)//针对每一个Field进行的解析
                                {
                                    string[] VTemp = CutOutStringByString(Field, new string[] { "16" });//特殊分隔符FD与V
                                    if (SegmentTemp.Length == 2)
                                    {
                                        string FieldName = HexStringToAsciiString(VTemp[0].Replace(" ", ""));
                                        string V = HexStringToAsciiString(VTemp[1].Replace(" ", ""));
                                        switch (FieldName)
                                        {

                                            case "Val": rqc.MCHC_Val = V; break;
                                            case "Unit": rqc.MCHC_Unit = V; break;
                                            default: break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            default: break;
                            #endregion
                        }
                    }
                }
                catch (Exception ) { }
            }
            try
            {
                string Json = Newtonsoft.Json.JsonConvert.SerializeObject(rqc);
                DataUpload.ReportAsyn.BeginInvoke(Json, MainStatic.GetDeviceSn(ComName), null, null);
            }
            catch (Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("数据将要转JSON异常", ex.Message);
            }
        }
        #endregion

        #region 其它辅助
        //根据一个字符串截取字符串
        private static string[] CutOutStringByString(string DesString, string[] flag)
        {
            try
            {
                string[] DataHexAry = DesString.Split(flag, StringSplitOptions.RemoveEmptyEntries);
                return DataHexAry;
            }
            catch (Exception ) { return null; }
        }
        //把一个16进制的字符串转换未ACRII码
        //private static string HexStringToAsciiString(string DesString)
        //{
        //    try
        //    {
        //        byte[] MDByteAry = ConvertData.HexToByte(DesString);
        //        return Encoding.ASCII.GetString(MDByteAry);
        //    }
        //    catch (Exception ) { return ""; }
        //}
        private static string HexStringToAsciiString(string DesString)
        {
            try
            {
                byte[] MDByteAry = ConvertData.HexToByte(DesString);
                return Encoding.Default.GetString(MDByteAry);
            }
            catch (Exception) { return ""; }
        }
        #endregion

    }
}
