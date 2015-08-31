using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insights.Tests.WebTestManagement.Models
{
    public static class WebTestLocations
    {
        /// <summary>
        /// Location - Chicago, IL USA.
        /// </summary>
        public static readonly WebTestLocation USChicagoIL = new WebTestLocation("us-il-ch1-azr");

        /// <summary>
        /// Location - San Antonio, TX USA.
        /// </summary>
        public static readonly WebTestLocation USSanAntonioTX = new WebTestLocation("us-tx-sn1-azr");

        /// <summary>
        /// Location - San Jose, CA USA.
        /// </summary>
        public static readonly WebTestLocation USSanJoseCA = new WebTestLocation("us-ca-sjc-azr");

        /// <summary>
        /// Location - Ashburn, VA USA.
        /// </summary>
        public static readonly WebTestLocation USAshburnVA = new WebTestLocation("us-va-ash-azr");

        /// <summary>
        /// Location - Dublin, Ireland.
        /// </summary>
        public static readonly WebTestLocation IEDublin = new WebTestLocation("emea-gb-db3-azr");

        /// <summary>
        /// Location - Amsterdam, Netherlands.
        /// </summary>
        public static readonly WebTestLocation NLAmsterdam = new WebTestLocation("emea-nl-ams-azr");

        /// <summary>
        /// Location - Hongkong.
        /// </summary>
        public static readonly WebTestLocation HKHongkong = new WebTestLocation("apac-hk-hkn-azr");

        /// <summary>
        /// Location - Singapore.
        /// </summary>
        public static readonly WebTestLocation SGSingapore = new WebTestLocation("apac-sg-sin-azr");
    }
}
