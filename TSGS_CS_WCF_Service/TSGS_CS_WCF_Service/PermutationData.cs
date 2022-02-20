using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSGS_CS_WCF_Service
{
    public class PermutationData
    {
        private int _CID;
        private int _RNR;
        private int _PID;
        private int _Sequence_Number;
        private int _Original_Sequence_Number;
        private int _Permutation_Number;
        private int _Paired;
        private int _Recordnr;

        public int CID { get { return _CID; } set { _CID = value; } }
        public int RNR { get { return _RNR; } set { _RNR = value; } }
        public int PID { get { return _PID; } set { _PID = value; } }
        public int Sequence_Number { get { return _Sequence_Number; } set { _Sequence_Number = value; } }
        public int Original_Sequence_Number { get { return _Original_Sequence_Number; } set { _Original_Sequence_Number = value; } }
        public int Permutation_Number { get { return _Permutation_Number; } set { _Permutation_Number = value; } }
        public int Paired { get { return _Paired; } set { _Paired = value; } }
        public int Recordnr { get { return _Recordnr; } set { _Recordnr = value; } }
    }
}
