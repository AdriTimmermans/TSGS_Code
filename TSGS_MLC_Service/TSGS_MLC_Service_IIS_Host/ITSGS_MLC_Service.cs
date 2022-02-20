using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TSGS_MLC_Service
{
    [ServiceContract]
    public interface ITSGS_MLC_Service
    {
        [OperationContract]
        string GetMLCText(string Project, string Page, int TextId, int Language);
    }
}