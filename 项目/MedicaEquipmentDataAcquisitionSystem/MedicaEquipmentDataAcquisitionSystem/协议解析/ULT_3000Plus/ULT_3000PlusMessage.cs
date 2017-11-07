using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MEDAS
{
    public class ULT_3000PlusMessage
    {
        public UInt16 Length { set; get; }
        public byte Type { set; get; }
        public byte DataType { set; get; }

        //20个参数  先按照例文中的顺序搞
        public UInt16 WBC { set; get; }
        public UInt16 LYPercentage { set; get; }
        public UInt16 MOPercentage { set; get; }
        public UInt16 GRPercentage { set; get; }
        public UInt16 LYValue { set; get; }
        public UInt16 MOValue { set; get; }
        public UInt16 GRValue { set; get; }
        public UInt16 RBC { set; get; }
        public UInt16 HCT { set; get; }
        public UInt16 MCV { set; get; }
        public UInt16 MCH { set; get; }
        public UInt16 MCHC { set; get; }
        public UInt16 RDW_cv { set; get; }
        public UInt16 RDW_sd { set; get; }
        public UInt16 PLT { set; get; }
        public UInt16 MPV { set; get; }
        public UInt16 PDW { set; get; }
        public UInt16 PCT { set; get; }
        public UInt16 P_LCR { set; get; }
        public UInt16 P_LCC { set; get; }

        //直方图
        public byte[] WBCHistogram { set; get; } //256字节
        public byte[] RBCHistogram { set; get; } //256字节
        public byte[] PLTHistogram { set; get; } //128字节

        //测量标杆
        public UInt32 WBC_SurveyorPole { set; get; }
        public UInt16 RBC_SurveyorPole { set; get; }
        public UInt16 PLT_SurveyorPole { set; get; }

        //报警值
        public byte R1_Alarm { set; get; }
        public byte R2_Alarm { set; get; }
        public byte R3_Alarm { set; get; }
        public byte R4_Alarm { set; get; }
        public byte PM_Alarm { set; get; }

        //计算时间
        public UInt16 WBC_CountingTime { set; get; }
        public UInt16 RBC_CountingTime { set; get; }

        //样品编号
        public string SampleNo { set; get; }

        //日期时间
        public DateTime DateTime_SampleNo { set; get; }
    }
}
