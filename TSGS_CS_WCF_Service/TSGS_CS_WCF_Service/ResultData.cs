using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSGS_CS_WCF_Service
{
    public class ResultData
    {
        private int _record_id;
        private int _Competitie_Id;
        private int _RondeNr;
        private int _Deelnemer_ID;
        private int _Tegenstander;
        private int _Kleur;
        private int _Resultaat;
        private float _ELO_Resultaat;
        private float _Competitie_Resultaat;
        private float _CorrectieOpElo;
        private float _CorrectieOpCompetitie;
        private int _Plaats_Op_Ranglijst;
        private string _Partijnaam;
        private DateTime _Rondedatum;
        private int _ChampionsgroupGameNumber;
        private float _Matchpunten;
        private int _Originele_Kroongroep;
        private byte _Was_Downfloat;
        private byte _Was_Upfloat;

        public int record_id { get { return _record_id; } set { _record_id = value; } }
        public int Competitie_Id { get { return _Competitie_Id; } set { _Competitie_Id = value; } }
        public int Rondernr { get { return _RondeNr; } set { _RondeNr = value; } }
        public int Deelnemer_ID { get { return _Deelnemer_ID; } set { _Deelnemer_ID = value; } }
        public int Tegenstander { get { return _Tegenstander; } set { _Tegenstander = value; } }
        public int Kleur { get { return _Kleur; } set { _Kleur = value; } }
        public int  Resultaat { get { return _Resultaat; } set { _Resultaat = value; } }
        public float ELO_Resultaat { get { return _ELO_Resultaat; } set { _ELO_Resultaat = value; } }
        public float Competitie_Resultaat { get { return _Competitie_Resultaat; } set { _Competitie_Resultaat = value; } }
        public float CorrectieOpElo { get { return _CorrectieOpElo; } set { _CorrectieOpElo = value; } }
        public float CorrectieOpCompetitie { get { return _CorrectieOpCompetitie; } set { _CorrectieOpCompetitie = value; } }
        public int Plaats_Op_Ranglijst { get { return _Plaats_Op_Ranglijst; } set { _Plaats_Op_Ranglijst = value; } }
        public string Partijnaam { get { return _Partijnaam; } set { _Partijnaam = value; } }
        public DateTime Rondedatum { get { return _Rondedatum; } set { _Rondedatum = value; } }
        public int ChampionsgroupGameNumber { get { return _ChampionsgroupGameNumber; } set { _ChampionsgroupGameNumber = value; } }
        public float Matchpunten { get { return _Matchpunten; } set { _Matchpunten = value; } }
        public int Originele_Kroongroep { get { return _Originele_Kroongroep; } set { _Originele_Kroongroep = value; } }
        public byte Was_Downfloat { get { return _Was_Downfloat; } set { _Was_Downfloat = value; } }
        public byte Was_Upfloat { get { return _Was_Upfloat; } set { _Was_Upfloat = value; } }
    }
}
