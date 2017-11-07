using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolAPI;

namespace MEDAS
{
    public class Bm21B
    {
        //拆MD
        public static void ProtocolAnalysis(byte[] frame,string ComName)
        {
            try
            {
                string dataHexString = ConvertData.ToHexString_Space(frame, 0, frame.Length);
                string[] stringSeparators = new string[] { "49 44 3A", "45 3A" };
                //判断起始符+版本号进行分割包
                string[] DataHexAry = dataHexString.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                if (DataHexAry.Length > 0)
                {
                    Bm21BMessage bm = new Bm21BMessage();
                    foreach (string frameTemp in DataHexAry)
                    {
                        if (!frameTemp.Contains("42 4C 4D") && frameTemp.Contains("44 61 74 61"))
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append("49 44 3A").Append(frameTemp).Append("45 3A");
                            string resultString = str.ToString();
                            stringSeparators = new string[] { " 0A 0D " };
                            string[] trem = resultString.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                            if (trem.Length > 0)
                            {
                                foreach (string tremTemp in trem)
                                {
                                    byte[] byteTemp = ConvertData.HexToByte(tremTemp.Replace(" ", ""));
                                    string tremTempAscii = Encoding.ASCII.GetString(byteTemp);
                                    int index = 0;
                                    if (tremTempAscii.Contains(":"))
                                        index = tremTempAscii.IndexOf(":");
                                    else
                                        index = tremTempAscii.IndexOf(" ");
                                    string key = tremTempAscii.Substring(0, index);
                                    string value = "";
                                    if(tremTempAscii.Length>index+1)
                                        value = tremTempAscii.Substring(index+1).Trim();
                                    GetKeyValue(ref bm, key, value);
                                }
                            }
                        }
                    }
                    try
                    {
                        string Json = Newtonsoft.Json.JsonConvert.SerializeObject(bm);
                        DataUpload.ReportAsyn.BeginInvoke(Json, MainStatic.GetDeviceSn(ComName), null, null);
                    }
                    catch (Exception ex)
                    {
                        ToolAPI.XMLOperation.WriteLogXmlNoTail("数据将要转JSON异常", ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("BC_1800.ProtocolAnalysis外层数据解析出现异常：", ee.Message);
            }
        }


        static void GetKeyValue(ref Bm21BMessage bm, string key, string value)
        {
            switch (key)
            {
                //基础
                case "ID": bm.ID = value; break;
                case "Data": bm.Data = value; break;
                case "MCH": bm.MCH = value; break;
                case "MCHC": bm.MCHC = value; break;
                case "Note": bm.Note = value; break;
                case "Serial": bm.Serial = value; break;
                case "Temp": bm.Temp = value; break;
                case "Flow": bm.Flow = value; break;
                case "Rode": bm.Rode = value; break;
                //红细胞
                case "RBC": bm.RBC_RBC = value; break;
                case "HCT": bm.RBC_HCT = value; break;
                case "MCV": bm.RBC_MCV = value; break;
                case "RDWSD": bm.RBC_RDWSD = value; break;
                case "RDWCV": bm.RBC_RDWCV = value; break;
                case "PLT": bm.RBC_PLT = value; break;
                case "PCT": bm.RBC_PCT = value; break;
                case "MPV": bm.RBC_MPV = value; break;
                case "PDW": bm.RBC_PDW = value; break;
                case "P-LCR": bm.RBC_P_LCR = value; break;
                //白细胞
                case "WBC": bm.WBC_WBC = value; break;
                case "LYM#": bm.WBC_LYMValue = value; break;
                case "MON#": bm.WBC_MONValue = value; break;
                case "GRA#": bm.WBC_GRAValue = value; break;
                case "LYM%": bm.WBC_LYMPercentage = value; break;
                case "MON%": bm.WBC_MONPercentag = value; break;
                case "GRA%": bm.WBC_GRAPercentag = value; break;
                case "HGB": bm.WBC_HGB = value; break;
                default: break;
            }
        }
    }
}
