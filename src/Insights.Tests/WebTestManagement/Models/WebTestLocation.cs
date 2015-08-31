using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insights.Tests.WebTestManagement.Models
{
    public class WebTestLocation
    {
        public string Id { get; private set; }

        public WebTestLocation(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }

            Id = id;
        }
    }
}
