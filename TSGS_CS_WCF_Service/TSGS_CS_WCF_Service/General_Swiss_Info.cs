using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSGS_CS_WCF_Service
{
    public class General_Swiss_Info
    {
        private int _Toernooi_Id;
        private DateTime _Aanmaakdatum;
        private string _Toernooi_Naam;
        private byte[] _Toernooi_Logo;
        private string _Hoofdsponsor;
        private string _Subsponsors;
        private int _Aantal_Ronden;
        private int _Aantal_Partijen_Per_Uitslag;
        private int _Indelings_Modus;
        private int _Aantal_Rating_Groepen;
        private byte[] _ProfilePicture;
        private int _Aantal_Invoerpunten;
        private int _Decentrale_Invoer_Spread;
        private int _Decentrale_Invoer_Maximaal;
        private int _KFactor;
        private int _Restrictie_Ronden;
        private int _Restrictie_Rating_Grens;
        private string _Website_Basis;
        private string _Website_Template;
        private string _Website_Competitie;
        private string _Client_FTP_Host;
        private string _Client_FTP_UN;
        private string _Client_FTP_PW;
        private int _CurrentState;
        private int _Laatste_Ronde;
        private int _Competitie_Type;
        private int _PartijenPerRonde;

        public int Toernooi_Id { get { return _Toernooi_Id; } set { _Toernooi_Id = value; } }
        public DateTime Aanmaakdatum { get { return _Aanmaakdatum; } set { _Aanmaakdatum = value; } }
        public string Toernooi_Naam { get { return _Toernooi_Naam; } set { _Toernooi_Naam = value; } }
        public byte[] Toernooi_Logo { get { return _Toernooi_Logo; } set { _Toernooi_Logo = value; } }
        public string Hoofdsponsor { get { return _Hoofdsponsor; } set { _Hoofdsponsor = value; } }
        public string Subsponsors { get { return _Subsponsors; } set { _Subsponsors = value; } }
        public int Aantal_Ronden { get { return _Aantal_Ronden; } set { _Aantal_Ronden = value; } }
        public int Laatste_Ronde { get { return _Laatste_Ronde; } set { _Laatste_Ronde = value; } }
        public int Aantal_Partijen_Per_Uitslag { get { return _Aantal_Partijen_Per_Uitslag; } set { _Aantal_Partijen_Per_Uitslag = value; } }
        public int Indelings_Modus { get { return _Indelings_Modus; } set { _Indelings_Modus = value; } }
        public int Aantal_Rating_Groepen { get { return _Aantal_Rating_Groepen; } set { _Aantal_Rating_Groepen = value; } }
        public byte[] ProfilePicture { get { return _ProfilePicture; } set { _ProfilePicture = value; } }
        public int Aantal_Invoerpunten { get { return _Aantal_Invoerpunten; } set { _Aantal_Invoerpunten = value; } }
        public int Decentrale_Invoer_Spread { get { return _Decentrale_Invoer_Spread; } set { _Decentrale_Invoer_Spread = value; } }
        public int Decentrale_Invoer_Maximaal { get { return _Decentrale_Invoer_Maximaal; } set { _Decentrale_Invoer_Maximaal = value; } }
        public int KFactor { get { return _KFactor; } set { _KFactor = value; } }
        public int Restrictie_Ronden { get { return _Restrictie_Ronden; } set { _Restrictie_Ronden = value; } }
        public int Restrictie_Rating_Grens { get { return _Restrictie_Rating_Grens; } set { _Restrictie_Rating_Grens = value; } }
        public string Website_Basis { get { return _Website_Basis; } set { _Website_Basis = value; } }
        public string Website_Template { get { return _Website_Template; } set { _Website_Template = value; } }
        public string Website_Competitie { get { return _Website_Competitie; } set { _Website_Competitie = value; } }
        public string Client_FTP_Host { get { return _Client_FTP_Host; } set { _Client_FTP_Host = value; } }
        public string Client_FTP_UN { get { return _Client_FTP_UN; } set { _Client_FTP_UN = value; } }
        public string Client_FTP_PW { get { return _Client_FTP_PW; } set { _Client_FTP_PW = value; } }
        public int CurrentState { get { return _CurrentState; } set { _CurrentState = value; } }
        public int Competitie_Type { get { return _Competitie_Type; } set { _Competitie_Type = value; } }
        public int PartijenPerRonde { get { return _PartijenPerRonde; } set { _PartijenPerRonde = value; } }
    }
}

