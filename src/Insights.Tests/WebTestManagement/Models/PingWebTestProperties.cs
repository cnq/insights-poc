using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insights.Tests.WebTestManagement.Models
{
    public class PingWebTestProperties : WebTestProperties
    {
        public override string Kind
        {
            get
            {
                return WebTestKinds.Ping;
            }
        }

        public PingWebTestProperties() : base()
        {

        }
    }
}
