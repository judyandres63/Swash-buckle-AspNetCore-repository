﻿using System.Net;
using System.Threading.Tasks;
using Xunit;
using ReDocApp = ReDoc;

namespace Swashbuckle.AspNetCore.IntegrationTests
{
    public class ReDocIntegrationTests
    {
        [Fact]
        public async Task RoutePrefix_RedirectsToIndexUrl()
        {
            var client = new TestSite(typeof(ReDocApp.Startup)).BuildClient();

            var response = await client.GetAsync("/api-docs");

            Assert.Equal(HttpStatusCode.MovedPermanently, response.StatusCode);
            Assert.Equal("http://localhost/api-docs/index.html", response.Headers.Location.ToString());
        }

        [Fact]
        public async Task IndexUrl_ReturnsEmbeddedVersionOfTheReDocUI()
        {
            var client = new TestSite(typeof(ReDocApp.Startup)).BuildClient();

            var indexResponse = await client.GetAsync("/api-docs/index.html");
            var jsResponse = await client.GetAsync("/api-docs/redoc.standalone.js");

            var indexContent = await indexResponse.Content.ReadAsStringAsync();
            Assert.Contains("Redoc.init", indexContent);
            Assert.Equal(HttpStatusCode.OK, jsResponse.StatusCode);
        }
    }
}