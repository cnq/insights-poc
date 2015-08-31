using Microsoft.VisualStudio.TestTools.WebTesting;

namespace Insights.Tests.WebTestManagement.Models
{
    public class PingWebTestConfiguration : WebTestConfiguration
    {
        DeclarativeWebTest _webTest;

        public override string WebTest
        {
            get
            {
                return _webTest.ToXml();
            }
        }

        public PingWebTestConfiguration(string url, int? expectedHttpStatusCode = 200)
        {
            _webTest = new DeclarativeWebTest { Proxy = "default" };
            var webTestRequest = new WebTestRequest(url)
            {
                ExpectedHttpStatusCode = expectedHttpStatusCode == null ? 0 : expectedHttpStatusCode.Value,
                IgnoreHttpStatusCode = expectedHttpStatusCode == null
            };
            _webTest.Items.Add(webTestRequest);
        }
    }
}
