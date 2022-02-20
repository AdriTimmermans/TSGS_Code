using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSGS_CS_WCF_Service
{
    public class SwissGamesData
    {
        private int _record_id;
        private int _Competition_Id;
        private int _Round_Number;
        private int _Id_Player_White;
        private float _Matchpoints_Player_White;
        private int _Id_Player_Black;
        private float _Matchpoints_Player_Black;
        private int _Game_Type;
        private int _Game_Result;
        private int _Pairing_Status_Level;
        private int _Sorting_Key;

        public int record_id { get { return _record_id; } set { _record_id = value; } }
        public int Competition_Id { get { return _Competition_Id; } set { _Competition_Id = value; } }
        public int Round_Number { get { return _Round_Number; } set { _Round_Number = value; } }
        public int Id_Player_White { get { return _Id_Player_White; } set { _Id_Player_White = value; } }
        public float Matchpoints_Player_White { get { return _Matchpoints_Player_White; } set { _Matchpoints_Player_White = value; } }
        public int Id_Player_Black { get { return _Id_Player_Black; } set { _Id_Player_Black = value; } }
        public float Matchpoints_Player_Black { get { return _Matchpoints_Player_Black; } set { _Matchpoints_Player_Black = value; } }
        public int Game_Type { get { return _Game_Type; } set { _Game_Type = value; } }
        public int Game_Result { get { return _Game_Result; } set { _Game_Result = value; } }
        public int Pairing_Status_Level { get { return _Pairing_Status_Level; } set { _Pairing_Status_Level = value; } }
        public int Sorting_Key { get { return _Sorting_Key; } set { _Sorting_Key = value; } } 

    }
}
