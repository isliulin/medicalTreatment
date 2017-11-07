using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MEDAS
{
    [Serializable]
    public class Bm21BMessage
    {
        public string ID { get; set; }
        public string Data { get; set; }
        public string MCH { get; set; }
        public string MCHC { get; set; }
        public string Note { get; set; }
        public string Serial { get; set; }
        public string Temp { get; set; }
        public string Flow { get; set; }
        public string Rode { get; set; }
       //红细胞
        public string RBC_RBC { get; set; }
        public string RBC_HCT { get; set; }
        public string RBC_MCV { get; set; }
        public string RBC_RDWSD { get; set; }
        public string RBC_RDWCV { get; set; }
        public string RBC_PLT { get; set; }
        public string RBC_PCT { get; set; }
        public string RBC_MPV { get; set; }
        public string RBC_PDW { get; set; }
        public string RBC_P_LCR { get; set; }

       //白细胞
        public string WBC_WBC { get; set; }
        public string WBC_LYMValue{ get; set; }
        public string WBC_MONValue { get; set; }
        public string WBC_GRAValue { get; set; }
        public string WBC_LYMPercentage { get; set; }
        public string WBC_MONPercentag { get; set; }
        public string WBC_GRAPercentag { get; set; }
        public string WBC_HGB { get; set; }
    }
}
