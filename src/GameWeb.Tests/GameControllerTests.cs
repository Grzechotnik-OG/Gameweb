using System;
using Xunit;
using GameWeb;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using System.Net;

namespace GameWeb.Tests
{
    public class GameControllerTests
    : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GameControllerTests(
            CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
        }

        [Fact]
        public async Task AuthorizationTest()
        {
            var result = await _client.GetAsync("/api/v1/games/1");
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }
    }
}
