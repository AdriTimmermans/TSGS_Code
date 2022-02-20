using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSGS_CS_WCF_Service
{
    public partial class AGVRoutingAlgorithm : Component
    {
        public AGVRoutingAlgorithm()
        {
            InitializeComponent();
        }

        public AGVRoutingAlgorithm(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
