using Hyak.Common;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Insights.Tests.WebTestManagement.Models
{
    public abstract class WebTestProperties
    {
        public abstract string Kind { get; }
        public string Name { get; set; }
        public string SyntheticMonitorId { get; set; }
        public bool Enabled { get; set; }
        public WebTestConfiguration Configuration { get; set; }
        public IList<WebTestLocation> Locations { get; set; }
        public bool RetryEnabled { get; set; }

        protected WebTestProperties()
        {
            Locations = new LazyList<WebTestLocation>();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static implicit operator string (WebTestProperties properties)
        {
            return properties.ToString();
        }
    }
}
