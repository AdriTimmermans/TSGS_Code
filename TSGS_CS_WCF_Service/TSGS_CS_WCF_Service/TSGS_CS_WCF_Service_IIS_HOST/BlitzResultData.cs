using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSGS_CS_WCF_Service
{
    public class BlitzResultData
    {
        private int _Competitie_Id;
        private int _RondeNr;
        private int _Deelnemer_ID;
        private DateTime _Rondedatum;
        private float _Strafpunten;
        private float _Matchpunten;
        private int _Groepnummer;
        private int _Deelnemer_Id;

        public int Competitie_Id { get { return _Competitie_Id; } set { _Competitie_Id = value; } }
        public int Rondernr { get { return _RondeNr; } set { _RondeNr = value; } }
        public int Deelnemer_ID { get { return _Deelnemer_ID; } set { _Deelnemer_ID = value; } }
        public DateTime Rondedatum { get { return _Rondedatum; } set { _Rondedatum = value; } }
        public float Strafpunten { get { return _Strafpunten; } set { _Strafpunten = value; } }
        public float Matchpunten { get { return _Matchpunten; } set { _Matchpunten = value; } }
        public int Groepnummer { get { return _Groepnummer; } set { _Groepnummer = value; } }
        public int Deelnemer_Id { get { return _Deelnemer_Id; } set { _Deelnemer_Id = value; } }
    }
}
