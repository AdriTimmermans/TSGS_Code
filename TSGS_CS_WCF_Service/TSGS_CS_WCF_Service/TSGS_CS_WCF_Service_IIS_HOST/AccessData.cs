using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSGS_CS_WCF_Service
{
    public class AccessData
    {
        private string _SessionID;
        private string _Huidige_Gebruiker;
        private DateTime _TimeStamp_Laatste_Load;

        public string SessionID { get { return _SessionID; } set { _SessionID = value; } }
        public string Huidige_Gebruiker { get { return _Huidige_Gebruiker; } set { _Huidige_Gebruiker = value; } }
        public DateTime TimeStamp_Laatste_Load { get { return _TimeStamp_Laatste_Load; } set { _TimeStamp_Laatste_Load = value; } }
    }
}
