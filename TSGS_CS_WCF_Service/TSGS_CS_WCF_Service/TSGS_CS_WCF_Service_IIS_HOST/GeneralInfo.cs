using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSGS_CS_WCF_Service
{
    public class GeneralInfo
    {
        private int _Competitie_Id;
        private string _Vereniging;
        private string _Naam_competitie;
        private DateTime _Aanmaakdatum;
        private int _Aantal_groepen;
        private int _Bonus_externe_wedstrijden;
        private int _Vrij_afmelden;
        private int _Strafpunten_afmelden;
        private int _Strafpunten_wegblijven;
        private int _Aantal_ronden;
        private int _Laatste_ronde;
        private int _KFactor;
        private int _Standaarddeviatie;
        private int _Aantal_Unieke_Ronden;
        private string _Intern_Basis;
        private string _Intern_Template;
        private string _Intern_Images;
        private string _Intern_Competitie;
        private string _Intern_Competitie_Images;
        private string _Website_Basis;
        private string _Website_Template;
        private string _Website_Competitie;
        private string _Website_Competitie_Images;
        private string _Client_FTP_Host;
        private string _Client_FTP_UN;
        private string _Client_FTP_PW;
        private int _Competitie_Type;
        private int _CurrentState;
        private byte[] _ProfilePicture;

        public int Competitie_Id { get { return _Competitie_Id; } set { _Competitie_Id = value; } }
        public string Vereniging { get { return _Vereniging; } set { _Vereniging = value; } }
        public string Naam_competitie { get { return _Naam_competitie; } set { _Naam_competitie = value; } }
        public DateTime Aanmaakdatum { get { return _Aanmaakdatum; } set { _Aanmaakdatum = value; } }
        public int Aantal_groepen { get { return _Aantal_groepen; } set { _Aantal_groepen = value; } }
        public int Bonus_externe_wedstrijden { get { return _Bonus_externe_wedstrijden; } set { _Bonus_externe_wedstrijden = value; } }
        public int Vrij_afmelden { get { return _Vrij_afmelden; } set { _Vrij_afmelden = value; } }
        public int Strafpunten_afmelden { get { return _Strafpunten_afmelden; } set { _Strafpunten_afmelden = value; } }
        public int Strafpunten_wegblijven { get { return _Strafpunten_wegblijven; } set { _Strafpunten_wegblijven = value; } }
        public int Aantal_ronden { get { return _Aantal_ronden; } set { _Aantal_ronden = value; } }
        public int Laatste_ronde { get { return _Laatste_ronde; } set { _Laatste_ronde = value; } }
        public int KFactor { get { return _KFactor; } set { _KFactor = value; } }
        public int Standaarddeviatie { get { return _Standaarddeviatie; } set { _Standaarddeviatie = value; } }
        public int Aantal_Unieke_Ronden { get { return _Aantal_Unieke_Ronden; } set { _Aantal_Unieke_Ronden = value; } }
        public string Intern_Basis { get { return _Intern_Basis; } set { _Intern_Basis = value; } }
        public string Intern_Template { get { return _Intern_Template; } set { _Intern_Template = value; } }
        public string Intern_Images { get { return _Intern_Images; } set { _Intern_Images = value; } }
        public string Intern_Competitie { get { return _Intern_Competitie; } set { _Intern_Competitie = value; } }
        public string Intern_Competitie_Images { get { return _Intern_Competitie_Images; } set { _Intern_Competitie_Images = value; } }
        public string Website_Basis { get { return _Website_Basis; } set { _Website_Basis = value; } }
        public string Website_Template { get { return _Website_Template; } set { _Website_Template = value; } }
        public string Website_Competitie { get { return _Website_Competitie; } set { _Website_Competitie = value; } }
        public string Website_Competitie_Images { get { return _Website_Competitie_Images; } set { _Website_Competitie_Images = value; } }
        public string Client_FTP_Host { get { return _Client_FTP_Host; } set { _Client_FTP_Host = value; } }
        public string Client_FTP_UN { get { return _Client_FTP_UN; } set { _Client_FTP_UN = value; } }
        public string Client_FTP_PW { get { return _Client_FTP_PW; } set { _Client_FTP_PW = value; } }
        public int Competitie_Type { get { return _Competitie_Type; } set { _Competitie_Type = value; } }
        public int CurrentState { get { return _CurrentState; } set { _CurrentState = value; } }
        public byte[] ProfilePicture { get { return _ProfilePicture; } set { _ProfilePicture = value; } }
    }
}
