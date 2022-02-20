using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WCF_Host
{
    class Program
    {
        static void Main()
        {
            using (ServiceHost host = new ServiceHost(typeof(TSGS_CS_WCF_Service.TSGS_CS_WCF_Service)))
            {
                host.Open();
                Console.WriteLine("TSGS_CS_WCF_Service has started");
                Console.ReadLine ();
            }

        }
    }
}
