using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolAPI;

namespace MEDAS
{
    public class Bm200
    {
        //拆MD
        public static void ProtocolAnalysis(byte[] frame,string ComName)
        {
            try
            {
                string dataHexString = ConvertData.ToHexString_Space(frame, 0, frame.Length);
                //判断起始符+版本号进行分割包
                Bm200Message bm = new Bm200Message();
                string[] stringSeparators = new string[] { "0A 0D" };
                string[] trem = dataHexString.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                if (trem.Length > 0)
                {
                    //BM200  Seq.No.    Pat.ID  SG   pH  LEU   ERY  NIT   KET  BIL  UBG   PRO  GLU
                    bool isPat_ID = false;
                    foreach (string tremTemp in trem)
                    {
                        byte[] byteTemp = ConvertData.HexToByte(tremTemp.Replace(" ", ""));
                        string tremTempAscii = Encoding.ASCII.GetString(byteTemp).Trim();
                        if (tremTempAscii.Contains("Pat.ID"))
                            isPat_ID = true;
                        int index = 0;
                            index = tremTempAscii.IndexOf(" ");
                        if (index >= 0)
                        {
                            if (isPat_ID)
                            {
                                bm.checktime = tremTempAscii;
                                isPat_ID = false;
                            }
                            else
                            {
                                string key = tremTempAscii.Substring(0, index);
                                string value = "";
                                if (tremTempAscii.Length > index + 1)
                                    value = tremTempAscii.Substring(index + 1).Trim();
                                GetKeyValue(ref bm, key, value);
                            }
                        }
                        else bm.Pat_ID = "";
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
            catch (Exception ee)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("BC_1800.ProtocolAnalysis外层数据解析出现异常：", ee.Message);
            }
        }


        static void GetKeyValue(ref Bm200Message bm, string key, string value)
        {
            switch (key)
            {
                //BM200  Seq.No.    Pat.ID  SG   pH  LEU   ERY  NIT   KET  BIL  UBG   PRO  GLU
                //基础
                case "BM200": bm.BM200 = value; break;
                case "Seq.No.": bm.Seq_No = value; break;
                case "Pat.ID": bm.Pat_ID = value; break;
                case "SG": bm.SG = value; break;
                case "pH": bm.pH = value; break;
                case "LEU": bm.LEU = value; break;
                case "ERY": bm.ERY = value; break;
                case "NIT": bm.NIT = value; break;
                case "KET": bm.KET = value; break;
                case "BIL": bm.BIL = value; break;
                case "UBG": bm.UBG = value; break;
                case "PRO": bm.PRO = value; break;
                case "GLU": bm.GLU = value; break;
                default: break;
            }
        }
    }
}
