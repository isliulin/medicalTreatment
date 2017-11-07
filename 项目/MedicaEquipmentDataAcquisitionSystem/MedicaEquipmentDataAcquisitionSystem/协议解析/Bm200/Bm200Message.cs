using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MEDAS
{
    [Serializable]
    public class Bm200Message
    {
        public string BM200 { get; set; }
        public string Seq_No { get; set; }
        public string Pat_ID { get; set; }
        public string checktime { get; set; }
        public string SG { get; set; }
        public string pH { get; set; }
        public string LEU { get; set; }
        public string ERY { get; set; }
        public string NIT { get; set; }
        public string KET { get; set; }
        public string BIL { get; set; }
        public string UBG { get; set; }
        public string PRO { get; set; }
        public string GLU { get; set; }
    }
}
