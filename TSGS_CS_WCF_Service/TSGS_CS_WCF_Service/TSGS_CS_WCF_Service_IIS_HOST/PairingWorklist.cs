using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSGS_CS_WCF_Service
{
    public class PairingWorklist
    {
        private int _record_id;
        private int _Speler_Id;
        private float _Start_Competition_Points; 
        private float _Competition_points;
        private int _Color_Balance;
        private int _Round_Free;
        private int _Mandatory_White;
        private int _Mandatory_Black;
        private float _Percentage;
        private float _Gain_Loss;
        private int _Competition_Id;
        private int _Round_Number;
        private float _Start_Rating;
        private float _Current_Rating;
        private float _Rating_Gain;
        private int _Aantal_Afmeldingen;
        private float _Aantal_Punten;
        private int _Aantal_Partijen;

        public int record_id { get { return  _record_id; } set {  _record_id = value; }}
        public int Speler_Id { get { return  _Speler_Id; } set {  _Speler_Id = value; }}
        public float Competition_points { get { return _Competition_points; } set { _Competition_points = value; } }
        public float Start_Competition_Points { get { return _Start_Competition_Points; } set { _Start_Competition_Points = value; } }
        public int Color_Balance { get { return  _Color_Balance; } set {  _Color_Balance = value; }}
        public int Round_Free { get { return  _Round_Free; } set {  _Round_Free = value; }}
        public int Mandatory_White { get { return  _Mandatory_White; } set {  _Mandatory_White = value; }}
        public int Mandatory_Black { get { return  _Mandatory_Black; } set {  _Mandatory_Black = value; }}
        public float Percentage { get { return  _Percentage; } set {  _Percentage = value; }}
        public float Gain_Loss { get { return  _Gain_Loss; } set {  _Gain_Loss = value; }}
        public int Competition_Id { get { return  _Competition_Id; } set {  _Competition_Id = value; }}
        public int Round_Number { get { return  _Round_Number; } set {  _Round_Number = value; }}
        public float Start_Rating { get { return  _Start_Rating; } set {  _Start_Rating = value; }}
        public float Current_Rating { get { return  _Current_Rating; } set {  _Current_Rating = value; }}
        public float Rating_Gain { get { return  _Rating_Gain; } set {  _Rating_Gain = value; }}
        public int Aantal_Afmeldingen { get { return  _Aantal_Afmeldingen; } set {  _Aantal_Afmeldingen = value; }}
        public float Aantal_Punten { get { return  _Aantal_Punten; } set {  _Aantal_Punten = value; }}
        public int Aantal_Partijen { get { return _Aantal_Partijen; } set { _Aantal_Partijen = value; }}
    }
}
