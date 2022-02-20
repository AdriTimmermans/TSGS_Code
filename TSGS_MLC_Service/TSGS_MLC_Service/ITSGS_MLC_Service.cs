using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace TSGS_MLC_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITSGS_MLC_Service" in both code and config file together.
    [ServiceContract]
    public interface ITSGS_MLC_Service
    {
        [OperationContract]
        string GetMLCText(string Project, string Page, int Language, int OrderNumber);

        [OperationContract]
        DataSet GetLanguageList();

        [OperationContract]
        void TranslateLanguages();
    }
}