using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSGS_CS_WCF_Service
{
    public class CompetitionManager
    {
        private string _CompetitionManagerName;
        private string _Password;
        private int _CompetitionManagerId;
        private string _EmailAddress;
        private int _UserPrivileges;
        private int _Fontsize;

        public string CompetitionManagerName
        {
            get { return _CompetitionManagerName; }
            set { _CompetitionManagerName = value; }
        }

        public string Password
        {
            get { return _Password;}
            set { _Password = value;}
        }

        public int UserPrivileges
        {
            get { return _UserPrivileges; }
            set { _UserPrivileges = value; }
        }

        public int Fontsize
        {
            get { return _Fontsize; }
            set { _Fontsize = value; }
        }
        public int CompetitionManagerId
        {
            get { return _CompetitionManagerId; }
            set { _CompetitionManagerId = value; }
        }

        public string EmailAddress
        {
            get { return _EmailAddress; }
            set { _EmailAddress = value; }
        }
    }
}
