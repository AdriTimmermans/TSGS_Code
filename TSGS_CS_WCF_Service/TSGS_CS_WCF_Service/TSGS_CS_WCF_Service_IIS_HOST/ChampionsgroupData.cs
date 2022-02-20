using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSGS_CS_WCF_Service
{
    public class ChampionsgroupData
    {
        private int _Player_Id;
        private int _Competition_Id;
        private int _StartRating;
        private int _StartCompetitionPoints;
        private int _LotNumber;

        public int Player_Id
        {
            get { return _Player_Id; }
            set { _Player_Id = value; }
        }

        public int Competition_Id
        {
            get { return _Competition_Id; }
            set { _Competition_Id = value; }
        }

        public int StartRating
        {
            get { return _StartRating; }
            set { _StartRating = value; }
        }

        public int StartCompetitionPoints
        {
            get { return _StartCompetitionPoints; }
            set { _StartCompetitionPoints = value; }
        }

        public int LotNumber
        {
            get { return _LotNumber; }
            set { _LotNumber = value; }
        }

    }
}
