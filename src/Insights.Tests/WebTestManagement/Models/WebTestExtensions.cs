using Microsoft.VisualStudio.TestTools.WebTesting;
using System.IO;

namespace Insights.Tests.WebTestManagement.Models
{
    public static class WebTestExtensions
    {
        public static string ToXml(this DeclarativeWebTest webTest)
        {
            string xml;
            using (var stream = new MemoryStream())
            using (var reader = new StreamReader(stream))
            {
                DeclarativeWebTestSerializer.Save(webTest, stream);
                stream.Position = 0;
                xml = reader.ReadToEnd();
            }
            return xml;
        }
    }
}
