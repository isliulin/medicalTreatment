using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolAPI;

namespace MEDAS
{
    public class ProtocolAnalysis
    {
        public static  void DataAnalysis(byte[] dataTemp,string ComName)
        {
            try
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("接收数据：",ComName+";"+ ConvertData.ToHexString(dataTemp, 0, dataTemp.Length));
                if (dataTemp[0] == 0x02)
                    ULT_3000Plus.ProtocolAnalysis(dataTemp,ComName);
                else if (dataTemp[0] == 0x05)
                    BC_1800.ProtocolAnalysis(dataTemp, ComName);
                else if(dataTemp[0] == 0x77 || dataTemp[0] == 0x49)
                    Bm21B.ProtocolAnalysis(dataTemp, ComName);
                else if (dataTemp[0] == 0x42)
                    Bm200.ProtocolAnalysis(dataTemp, ComName);
            }
            catch(Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail("DataAnalysis异常：",ex.Message);
            }
           
        }
    }
}
