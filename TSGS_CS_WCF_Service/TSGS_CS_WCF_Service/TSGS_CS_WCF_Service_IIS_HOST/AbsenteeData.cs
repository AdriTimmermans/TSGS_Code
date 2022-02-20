using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSGS_CS_WCF_Service
{
    public class AbsenteeData
    {
        private int _NI_Record_ID;
        private int _Speler_ID;
        private int _Rondenummer;
        private int _Afwezigheidscode;
        private int _Kroongroep_partijnummer;
        private int _Competitie_ID;


        public int NI_Record_ID
        {
            get { return _NI_Record_ID; }
            set { _NI_Record_ID = value; }
        }

        public int Speler_ID
        {
            get { return _Speler_ID; }
            set { _Speler_ID = value; }
        }

        public int Rondenummer
        {
            get { return _Rondenummer; }
            set { _Rondenummer = value; }
        }

        public int Afwezigheidscode
        {
            get { return _Afwezigheidscode; }
            set { _Afwezigheidscode = value; }
        }

        public int Kroongroep_partijnummer
        {
            get { return _Kroongroep_partijnummer; }
            set { _Kroongroep_partijnummer = value; }
        }

        public int Competitie_ID
        {
            get { return _Competitie_ID; }
            set { _Competitie_ID = value; }
        }
    }
}
