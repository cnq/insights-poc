using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Azure.Management.Resources;
using Microsoft.Azure;
using Microsoft.Azure.Management.Resources.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net;
using Insights.Tests.WebTestManagement.Models;
using Microsoft.Azure.Management.Insights;
using Microsoft.Azure.Management.Insights.Models;

namespace Insights.Tests
{
    [TestClass]
    public class CreatePingWebTest
    {
        private const string _subscriptionId = "";
        private const string _tenantDomain = "";
        private const string _clientId = "";
        private const string _username = "";
        private const string _password = "";

        [TestMethod]
        public void ItShouldCreateOrUpdateResources()
        {
            const string resourceGroupName = "AppInsigtsResourceGroup";
            const string insightsResourceName = "AppInsightsResource";
            const string webTestName = "YahooPingTest";
            const string webTestUrl = "http://www.yahoo.com";
            const string location = "centralus";

            var credentials = GetCloudCredentials();
            var resourceClient = new ResourceManagementClient(credentials);
            var registrationResult = resourceClient.Providers.RegisterAsync("microsoft.insights").Result;
            var insightsClient = new InsightsManagementClient(credentials);

            var insightsResource = CreateOrUpdateInsightsResource(
                resourceClient, 
                resourceGroupName, 
                insightsResourceName, 
                location);

            var webTest = CreateOrUpdateWebTest(
                resourceClient,
                resourceGroupName,
                insightsResource.Id,
                webTestName,
                webTestUrl,
                location);

            CreateOrUpdateAlert(
                insightsClient,
                resourceGroupName,
                insightsResource.Id,
                webTest.Id);

            Assert.IsNotNull(insightsResource);
            Assert.IsNotNull(webTest);
        }

        private static GenericResourceExtended CreateOrUpdateInsightsResource(
            ResourceManagementClient resourceClient, 
            string resourceGroupName, 
            string insightsResourceName, 
            string location)
        {
            var resourceGroupResult = resourceClient.ResourceGroups.CreateOrUpdateAsync(
                resourceGroupName, 
                new ResourceGroup(location)).Result;

            if(resourceGroupResult.StatusCode != HttpStatusCode.Created && resourceGroupResult.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to create resource group '{0}' (HTTP Status Code: {1}).",
                    resourceGroupName,
                    resourceGroupResult.StatusCode));
            }

            var resourceIdentity = new ResourceIdentity(
                insightsResourceName,
                "microsoft.insights/components",
                "2015-05-01");
            
            var resourceResult = resourceClient.Resources.CreateOrUpdateAsync(
                resourceGroupName,
                resourceIdentity,
                new GenericResource(location)).Result;

            if (resourceResult.StatusCode != HttpStatusCode.Created && resourceResult.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to create resource '{0}' (HTTP Status Code: {1}).",
                    insightsResourceName,
                    resourceResult.StatusCode));
            }

            return resourceResult.Resource;
        }

        private static GenericResourceExtended CreateOrUpdateWebTest(
            ResourceManagementClient resourceClient, 
            string resourceGroupName, 
            string insightsResourceUri, 
            string webTestName, 
            string webTestUrl,
            string location)
        {
            string insightsResourceName = GetNameFromUri(insightsResourceUri);
            string webTestMonitorId = string.Format("{0}-{1}", webTestName, insightsResourceName);

            var webTestResourceIdentity = new ResourceIdentity(
                webTestMonitorId,
                "microsoft.insights/webtests", 
                "2015-05-01");
            
            var webTestResource = new GenericResource(location)
            {
                Properties = new PingWebTestProperties
                {
                    Name = webTestName,
                    SyntheticMonitorId = webTestMonitorId,
                    Enabled = true,
                    Configuration = new PingWebTestConfiguration(webTestUrl),
                    Locations = { WebTestLocations.USSanAntonioTX },
                    RetryEnabled = true
                },
                Tags =
                {
                    { string.Format("hidden-link:{0}", insightsResourceUri), "Resource" }
                }
            };

            var webTestResourceCreateOrUpdateResult = resourceClient.Resources.CreateOrUpdateAsync(
                resourceGroupName,
                webTestResourceIdentity,
                webTestResource).Result;
            
            if (webTestResourceCreateOrUpdateResult.StatusCode != HttpStatusCode.Created && webTestResourceCreateOrUpdateResult.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to create resource '{0}' (HTTP Status Code: {1}).",
                    insightsResourceName,
                    webTestResourceCreateOrUpdateResult.StatusCode));
            }

            return webTestResourceCreateOrUpdateResult.Resource;
        }

        private static void CreateOrUpdateAlert(
            InsightsManagementClient insightsClient,
            string resourceGroupName,
            string insightsResourceUri,
            string webTestUri)
        {
            string webTestName = GetNameFromUri(webTestUri);
            string alertName = string.Format("{0}-alert", webTestName);

            var parameters = new RuleCreateOrUpdateParameters
            {
                Location = "East US",
                Properties = new Rule()
                {
                    Name = alertName,
                    Description = string.Empty,
                    IsEnabled = true,
                    LastUpdatedTime = DateTime.UtcNow,
                    Action = new RuleEmailAction
                    {
                        SendToServiceOwners = true
                    },
                    Condition = new LocationThresholdRuleCondition
                    {
                        DataSource = new RuleMetricDataSource
                        {
                            MetricName = "GSMT_AvRaW",
                            ResourceUri = webTestUri
                        },
                        FailedLocationCount = 1,
                        WindowSize = TimeSpan.FromMinutes(5)
                    },
                },
                Tags =
                {
                    { string.Format("hidden-link:{0}", insightsResourceUri), "Resource" },
                    { string.Format("hidden-link:{0}", webTestUri), "Resource" }
                }
            };

            var alertCreateOrUpdateResult = insightsClient.AlertOperations.CreateOrUpdateRuleAsync(
                resourceGroupName, 
                parameters).Result;
            
            if (alertCreateOrUpdateResult.StatusCode != HttpStatusCode.Created && alertCreateOrUpdateResult.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to create resource '{0}' (HTTP Status Code: {1}).",
                    insightsResourceUri,
                    alertCreateOrUpdateResult.StatusCode));
            }
        }

        private static SubscriptionCloudCredentials GetCloudCredentials()
        {
            // TODO: Use certificates instead of a hardcoded username and password to obtain the authentication token.
            var authenticationContext = new AuthenticationContext("https://login.windows.net/" + _tenantDomain);
            var authenticationResult = authenticationContext.AcquireToken("https://management.core.windows.net/", _clientId, new UserCredential(_username, _password));
            return new TokenCloudCredentials(_subscriptionId, authenticationResult.AccessToken);
        }

        private static string GetNameFromUri(string uri)
        {
            if(string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException("uri");
            }
            int lastIndexOfSlash = uri.LastIndexOf("/");
            string name = uri.Substring(lastIndexOfSlash + 1);
            return name;
        }
    }
}
