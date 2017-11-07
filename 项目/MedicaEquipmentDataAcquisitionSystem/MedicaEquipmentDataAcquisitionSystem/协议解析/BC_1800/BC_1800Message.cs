using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MEDAS
{
    #region 样本数据 正常计数结果数据包
    [Serializable]
    public class SampleResult
    {
        //SampleInfo
        public string SampleInfo_SampleID { get; set; }
        public string SampleInfo_Mode { get; set; }
        public string SampleInfo_TestTime { get; set; }
        public string SampleInfo_Name { get; set; }
        public string SampleInfo_Group { get; set; }
        public string SampleInfo_Gender { get; set; }
        public string SampleInfo_AgeVal { get; set; }
        public string SampleInfo_AgeType { get; set; }
        public string SampleInfo_Dept{ get; set; }
        public string SampleInfo_ChartNo { get; set; }
        public string SampleInfo_BedNo { get; set; }
        public string SampleInfo_Sender { get; set; }
        public string SampleInfo_Tester { get; set; }
        public string SampleInfo_Checker { get; set; }
        //WBC
        public string WBC_Val { get; set; }
        public string WBC_Low { get; set; }
        public string WBC_High { get; set; }
        public string WBC_Unit { get; set; }
        public string WBC_Flag { get; set; }
        //Lymph#
        public string LymphValue_Val { get; set; }
        public string LymphValue_Low { get; set; }
        public string LymphValue_High { get; set; }
        public string LymphValue_Unit { get; set; }
        public string LymphValue_Flag { get; set; }
        //Mid#
        public string MidValue_Val { get; set; }
        public string MidValue_Low { get; set; }
        public string MidValue_High { get; set; }
        public string MidValue_Unit { get; set; }
        public string MidValue_Flag { get; set; }
        //Gran#
        public string GranValue_Val { get; set; }
        public string GranValue_Low { get; set; }
        public string GranValue_High { get; set; }
        public string GranValue_Unit { get; set; }
        public string GranValue_Flag { get; set; }
        //Lymph%
        public string LymphPercentage_Val { get; set; }
        public string LymphPercentage_Low { get; set; }
        public string LymphPercentage_High { get; set; }
        public string LymphPercentage_Unit { get; set; }
        public string LymphPercentage_Flag { get; set; }
        //Mid%
        public string MidPercentage_Val { get; set; }
        public string MidPercentage_Low { get; set; }
        public string MidPercentage_High { get; set; }
        public string MidPercentage_Unit { get; set; }
        public string MidPercentage_Flag { get; set; }
        //Gran%
        public string GranPercentage_Val { get; set; }
        public string GranPercentage_Low { get; set; }
        public string GranPercentage_High { get; set; }
        public string GranPercentage_Unit { get; set; }
        public string GranPercentage_Flag { get; set; }
        //HGB
        public string HGB_Val { get; set; }
        public string HGB_Low { get; set; }
        public string HGB_High { get; set; }
        public string HGB_Unit { get; set; }
        public string HGB_Flag { get; set; }
        //RBC
        public string RBC_Val { get; set; }
        public string RBC_Low { get; set; }
        public string RBC_High { get; set; }
        public string RBC_Unit { get; set; }
        public string RBC_Flag { get; set; }
        //HCT
        public string HCT_Val { get; set; }
        public string HCT_Low { get; set; }
        public string HCT_High { get; set; }
        public string HCT_Unit { get; set; }
        public string HCT_Flag { get; set; }
        //MCV
        public string MCV_Val { get; set; }
        public string MCV_Low { get; set; }
        public string MCV_High { get; set; }
        public string MCV_Unit { get; set; }
        public string MCV_Flag { get; set; }
        //MCH
        public string MCH_Val { get; set; }
        public string MCH_Low { get; set; }
        public string MCH_High { get; set; }
        public string MCH_Unit { get; set; }
        public string MCH_Flag { get; set; }
        //MCHC
        public string MCHC_Val { get; set; }
        public string MCHC_Low { get; set; }
        public string MCHC_High { get; set; }
        public string MCHC_Unit { get; set; }
        public string MCHC_Flag { get; set; }
        //RDWCV
        public string RDWCV_Val { get; set; }
        public string RDWCV_Low { get; set; }
        public string RDWCV_High { get; set; }
        public string RDWCV_Unit { get; set; }
        public string RDWCV_Flag { get; set; }
        //RDWSD
        public string RDWSD_Val { get; set; }
        public string RDWSD_Low { get; set; }
        public string RDWSD_High { get; set; }
        public string RDWSD_Unit { get; set; }
        public string RDWSD_Flag { get; set; }
        //PLT
        public string PLT_Val { get; set; }
        public string PLT_Low { get; set; }
        public string PLT_High { get; set; }
        public string PLT_Unit { get; set; }
        public string PLT_Flag { get; set; }
        //MPV
        public string MPV_Val { get; set; }
        public string MPV_Low { get; set; }
        public string MPV_High { get; set; }
        public string MPV_Unit { get; set; }
        public string MPV_Flag { get; set; }
        //PDW
        public string PDW_Val { get; set; }
        public string PDW_Low { get; set; }
        public string PDW_High { get; set; }
        public string PDW_Unit { get; set; }
        public string PDW_Flag { get; set; }
        //PCT
        public string PCT_Val { get; set; }
        public string PCT_Low { get; set; }
        public string PCT_High { get; set; }
        public string PCT_Unit { get; set; }
        public string PCT_Flag { get; set; }
        //AlarmFlag
        public string AlarmFlag_Rm { get; set; }
        public string AlarmFlag_R1 { get; set; }
        public string AlarmFlag_R2 { get; set; }
        public string AlarmFlag_R3 { get; set; }
        public string AlarmFlag_R4 { get; set; }
        public string AlarmFlag_Pm { get; set; }
        public string AlarmFlag_Ps { get; set; }
        public string AlarmFlag_P1 { get; set; }
        ////SepLine
        //public string SepLine_SepWBCLyLeft { get; set; }
        //public string SepLine_SepWBCLyMid { get; set; }
        //public string SepLine_SepWBCGranMid { get; set; }
        //public string SepLine_SepWBCGranRight { get; set; }
        //public string SepLine_SepRBCLef { get; set; }
        //public string SepLine_SepRBCRight { get; set; }
        //public string SepLine_SepPLTLeft { get; set; }
        //public string SepLine_SepPLTRight { get; set; }
        ////WBCHisto
        //public string WBCHisto_DataLen { get; set; }
        //public string WBCHisto_MeteDataLen { get; set; }
        //public byte[] WBCHisto_WHistoData { get; set; }
        ////RBCHisto
        //public string RBCHisto_DataLen { get; set; }
        //public string RBCHisto_MeteDataLen { get; set; }
        //public byte[] RBCHisto_RHistoData { get; set; }
        ////PLTHisto
        //public string PLTHisto_DataLen { get; set; }
        //public string PLTHisto_MeteDataLen { get; set; }
        //public byte[] PLTHisto_PHistoData { get; set; }
    }
    #endregion

    #region 标准质控
    [Serializable]
    public class StandardQualityControl
    {
        //StQCInfo
        public string StQCInfo_FileNo { get; set; }
        public string StQCInfo_LotNo { get; set; }
        public string StQCInfo_ExpDate { get; set; }
        //WBC
        public string WBC_Mean { get; set; }
        public string WBC_Range { get; set; }
        public string WBC_Unit { get; set; }
        //RBC
        public string RBC_Mean { get; set; }
        public string RBC_Range { get; set; }
        public string RBC_Unit { get; set; }
        //HGB
        public string HGB_Mean { get; set; }
        public string HGB_Range { get; set; }
        public string HGB_Unit { get; set; }
        //PLT
        public string PLT_Mean { get; set; }
        public string PLT_Range { get; set; }
        public string PLT_Unit { get; set; }
        //Lymph#
        public string LymphValue_Mean { get; set; }
        public string LymphValue_Range { get; set; }
        public string LymphValue_Unit { get; set; }
        //Lymph%
        public string LymphPercentage_Mean { get; set; }
        public string LymphPercentage_Range { get; set; }
        public string LymphPercentage_Unit { get; set; }
        //Gran#
        public string GranValue_Mean { get; set; }
        public string GranValue_Range { get; set; }
        public string GranValue_Unit { get; set; }
        //Gran%
        public string GranPercentage_Mean { get; set; }
        public string GranPercentage_Range { get; set; }
        public string GranhPercentage_Unit { get; set; }
        //HCT
        public string HCT_Mean { get; set; }
        public string HCT_Range { get; set; }
        public string HCT_Unit { get; set; }
        //MCV
        public string MCV_Mean { get; set; }
        public string MCV_Range { get; set; }
        public string MCV_Unit { get; set; }
        //MCH
        public string MCH_Mean { get; set; }
        public string MCH_Range { get; set; }
        public string MCH_Unit { get; set; }
        //MCHC
        public string MCHC_Mean { get; set; }
        public string MCHC_Range { get; set; }
        public string MCHC_Unit { get; set; }
    }
    #endregion

    #region 运行质控
    [Serializable]
    public class RunQualityControl
    {
        //RunQCInfo
        public string RunQCInfo_FileNo { get; set; }
        public string RunQCInfo_LotNo { get; set; }
        public string RunQCInfo_ExpDate { get; set; }
        //WBC
        public string WBC_Val { get; set; }
        public string WBC_Unit { get; set; }
        //RBC
        public string RBC_Val { get; set; }
        public string RBC_Unit { get; set; }
        //HGB
        public string HGB_Val { get; set; }
        public string HGB_Unit { get; set; }
        //PLT
        public string PLT_Val { get; set; }
        public string PLT_Unit { get; set; }
        //Lymph#
        public string LymphValue_Val { get; set; }
        public string LymphValue_Unit { get; set; }
        //Lymph%
        public string LymphPercentage_Val { get; set; }
        public string LymphPercentage_Unit { get; set; }
        //Gran#
        public string GranValue_Val { get; set; }
        public string GranValue_Unit { get; set; }
        //Gran%
        public string GranPercentage_Val { get; set; }
        public string GranhPercentage_Unit { get; set; }
        //HCT
        public string HCT_Val { get; set; }
        public string HCT_Unit { get; set; }
        //MCV
        public string MCV_Val { get; set; }
        public string MCV_Unit { get; set; }
        //MCH
        public string MCH_Val { get; set; }
        public string MCH_Unit { get; set; }
        //MCHC
        public string MCHC_Val { get; set; }
        public string MCHC_Unit { get; set; }
    }
    #endregion
}
