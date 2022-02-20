using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSGS_CS_WCF_Service
{
    public class GamesData
    {
        private int _record_id;
        private int _Competitie_Id;
        private int _RondeNr;
        private int _Id_Witspeler;
        private float _Wit_Winst;
        private float _Wit_Remise;
        private float _Wit_Verlies;
        private int _Id_Zwartspeler;
        private float _Zwart_Winst;
        private float _Zwart_Remise;
        private float _Zwart_Verlies;
        private int _Wedstrijdtype;
        private int _Wedstrijdresultaat;
        private int _Sorteerwaarde;
        private int _NumberChampionsgroupGame;

        public int record_id { get { return _record_id; } set { _record_id = value;}}
        public int Competitie_Id { get { return _Competitie_Id; } set { _Competitie_Id = value; } }
        public int Rondernr { get { return _RondeNr; } set { _RondeNr = value; } }
        public int Id_Witspeler { get { return _Id_Witspeler; } set { _Id_Witspeler = value; } }
        public float Wit_Winst { get { return _Wit_Winst; } set { _Wit_Winst = value; } }
        public float Wit_Remise { get { return _Wit_Remise; } set { _Wit_Remise = value; } }
        public float Wit_Verlies { get { return _Wit_Verlies; } set { _Wit_Verlies = value; } }
        public int Id_Zwartspeler { get { return _Id_Zwartspeler; } set { _Id_Zwartspeler = value; } }
        public float Zwart_Verlies { get { return _Zwart_Verlies; } set { _Zwart_Verlies = value; } }
        public float Zwart_Remise { get { return _Zwart_Remise; } set { _Zwart_Remise = value; } }
        public float Zwart_Winst { get { return _Zwart_Winst; } set { _Zwart_Winst = value; } }
        public int Wedstrijdtype { get { return _Wedstrijdtype; } set { _Wedstrijdtype = value;}}
        public int Wedstrijdresultaat { get { return _Wedstrijdresultaat; } set { _Wedstrijdresultaat = value; } }
        public int Sorteerwaarde { get { return _Sorteerwaarde; } set { _Sorteerwaarde = value; } }
        public int NumberChampionsgroupGame { get { return _NumberChampionsgroupGame; } set { _NumberChampionsgroupGame = value; } } 

    }
}
