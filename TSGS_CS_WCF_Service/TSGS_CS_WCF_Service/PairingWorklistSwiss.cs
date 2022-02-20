using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSGS_CS_WCF_Service
{
    public class PairingWorklistSwiss
    {
        private int _record_id;
        private int _Competition_Id;
        private int _Round_Number;
        private int _Speler_Id;
        private float _Start_Rating;
        private int _Number_of_Games; 
        private float _MatchPoints;
        private float _WP;
        private float _SB;
        private int _Color_Balance;
        private bool _Absolute_Color_Preference_White;
        private bool _Strong_Color_Preference_White;
        private bool _Mild_Color_Preference_White;
        private bool _Absolute_Color_Preference_Black;
        private bool _Strong_Color_Preference_Black;
        private bool _Mild_Color_Preference_Black;
        private bool _Had_Pair_Allocated_Bye;
        private bool _Is_In_Top_50_Percent;
        private bool _Last_Pairing_Upfloated;
        private int _Number_Upfloats;
        private bool _Last_Pairing_Downfloated;
        private int _Number_Downfloats;
        private float _Rating_Gain;

        public int record_id { get { return _record_id; } set { _record_id = value; } }
        public int Competition_Id { get { return _Competition_Id; } set { _Competition_Id = value; } }
        public int Round_Number { get { return _Round_Number; } set { _Round_Number = value; } }
        public int Speler_Id { get { return _Speler_Id; } set { _Speler_Id = value; } }
        public float Start_Rating { get { return _Start_Rating; } set { _Start_Rating = value; } }
        public int Number_of_Games { get { return _Number_of_Games; } set { _Number_of_Games = value; } }
        public float MatchPoints { get { return _MatchPoints; } set { _MatchPoints = value; } }
        public float WP { get { return _WP; } set { _WP = value; } }
        public float SB { get { return _SB; } set { _SB = value; } }
        public int Color_Balance { get { return _Color_Balance; } set { _Color_Balance = value; } }
        public bool Absolute_Color_Preference_White { get { return _Absolute_Color_Preference_White; } set { _Absolute_Color_Preference_White = value; } }
        public bool Strong_Color_Preference_White { get { return _Strong_Color_Preference_White; } set { _Strong_Color_Preference_White = value; } }
        public bool Mild_Color_Preference_White { get { return _Mild_Color_Preference_White; } set { _Mild_Color_Preference_White = value; } }
        public bool Absolute_Color_Preference_Black { get { return _Absolute_Color_Preference_Black; } set { _Absolute_Color_Preference_Black = value; } }
        public bool Strong_Color_Preference_Black { get { return _Strong_Color_Preference_Black; } set { _Strong_Color_Preference_Black = value; } }
        public bool Mild_Color_Preference_Black { get { return _Mild_Color_Preference_Black; } set { _Mild_Color_Preference_Black = value; } }
        public bool Had_Pair_Allocated_Bye { get { return _Had_Pair_Allocated_Bye; } set { _Had_Pair_Allocated_Bye = value; } }
        public bool Is_In_Top_50_Percent { get { return _Is_In_Top_50_Percent; } set { _Is_In_Top_50_Percent = value; } }
        public bool Last_Pairing_Upfloated { get { return _Last_Pairing_Upfloated; } set { _Last_Pairing_Upfloated = value; } }
        public int Number_Upfloats { get { return _Number_Upfloats; } set { _Number_Upfloats = value; } }
        public bool Last_Pairing_Downfloated { get { return _Last_Pairing_Downfloated; } set { _Last_Pairing_Downfloated = value; } }
        public int Number_Downfloats { get { return _Number_Downfloats; } set { _Number_Downfloats = value; } }
        public float Rating_Gain { get { return _Rating_Gain; } set { _Rating_Gain = value; } }

    }
}
