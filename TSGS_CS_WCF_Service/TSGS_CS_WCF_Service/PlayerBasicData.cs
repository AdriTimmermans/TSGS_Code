using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSGS_CS_WCF_Service
{
    public class PlayerBasicData
    {

	    private int _Speler_ID; 
	    private int _Competitie_Id;
        private string _Titel;
	    private string _Achternaam;
	    private string _Tussenvoegsel;
	    private string _Voorletters; 
	    private string _Roepnaam; 
	    private int _KNSBnummer;
        private int _FIDEnummer;
	    private string _Telefoonnummer;

        private float _KNSBrating;
        private float _FIDErating;
        private float _Rating;
        private float _SnelschaakRating;
        private float _RapidRating;
        private float _Startrating;
	    
        private sbyte _Team;
	    private sbyte _Clublid;
	    private sbyte _Deelnemer_teruggetrokken; 
	    private sbyte _Speelt_mee_sinds_ronde; 
	    private sbyte _Doet_mee_met_snelschaak; 
	    private int _Speelt_blitz_sinds_ronde; 
	    private sbyte _Vrijgeloot; 
	    private sbyte _Wants_Email; 
	    private string _Email_Address; 
	    private sbyte _Wants_SMS; 
	    private string _Mobile_Number; 
	    private sbyte _Member_Premier_Group;
        private byte[] _ProfilePicture;


        public int Speler_ID { get { return _Speler_ID; } set { _Speler_ID = value; } }
        public int Competitie_Id { get { return _Competitie_Id; } set { _Competitie_Id = value; } }
        public string Titel { get { return _Titel; } set { _Titel = value; } }
        public string Achternaam { get { return _Achternaam; } set { _Achternaam = value; } }
        public string Tussenvoegsel { get { return _Tussenvoegsel; } set { _Tussenvoegsel = value; } }
        public string Voorletters { get { return _Voorletters; } set { _Voorletters = value; } }
        public string Roepnaam { get { return _Roepnaam; } set { _Roepnaam = value; } }
        public int KNSBnummer { get { return _KNSBnummer; } set { _KNSBnummer = value; } }
        public int FIDEnummer { get { return _FIDEnummer; } set { _FIDEnummer = value; } }
        public float KNSBrating { get { return _KNSBrating; } set { _KNSBrating = value; } }
        public float FIDErating { get { return _FIDErating; } set { _FIDErating = value; } }
        public float Snelschaakrating { get { return _SnelschaakRating; } set { _SnelschaakRating = value; } }
        public float Rapidrating { get { return _RapidRating; } set { _RapidRating = value; } }
        public string Telefoonnummer { get { return _Telefoonnummer; } set { _Telefoonnummer = value; } }
        public float Startrating { get { return _Startrating; } set { _Startrating = value; } }
        public float Rating { get { return _Rating; } set { _Rating = value; } }
        public sbyte Team { get { return _Team; } set { _Team = value; } }
        public sbyte Clublid { get { return _Clublid; } set { _Clublid = value; } }
        public sbyte Deelnemer_teruggetrokken { get { return _Deelnemer_teruggetrokken; } set { _Deelnemer_teruggetrokken = value; } }
        public sbyte Speelt_mee_sinds_ronde { get { return _Speelt_mee_sinds_ronde; } set { _Speelt_mee_sinds_ronde = value; } }
        public sbyte Doet_mee_met_snelschaak { get { return _Doet_mee_met_snelschaak; } set { _Doet_mee_met_snelschaak = value; } }
        public int Speelt_blitz_sinds_ronde { get { return _Speelt_blitz_sinds_ronde; } set { _Speelt_blitz_sinds_ronde = value; } }
        public sbyte Vrijgeloot { get { return _Vrijgeloot; } set { _Vrijgeloot = value; } }
        public sbyte Wants_Email { get { return _Wants_Email; } set { _Wants_Email = value; } }
        public string Email_Address { get { return _Email_Address; } set { _Email_Address = value; } }
        public sbyte Wants_SMS { get { return _Wants_SMS; } set { _Wants_SMS = value; } }
        public string Mobile_Number { get { return _Mobile_Number; } set { _Mobile_Number = value; } }
        public sbyte Member_Premier_Group { get { return _Member_Premier_Group; } set { _Member_Premier_Group = value; } }
        public byte[] ProfilePicture { get { return _ProfilePicture; } set { _ProfilePicture = value; } }
    }
}
