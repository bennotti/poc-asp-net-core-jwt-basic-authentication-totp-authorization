using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using SampleProject.Api;
using SampleProject.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SampleProjectxUnitTest
{
    public class TotpControllerTest
    {
        private readonly HttpClient _client;
        public TotpControllerTest()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<SampleProject.Api.Startup>());
            _client = server.CreateClient();
        }
        [Fact(DisplayName = "should be pass and Ok status code")]
        [Trait("TotpControllerTest", "Defined")]
        public async Task ServiceTest_with_service_defined()
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/totp/");

            // Act
            var response = await _client.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();
            var dataResponse = JsonConvert.DeserializeObject<TotpKeyResponseDto>(content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(dataResponse.Key);
        }
    }
}
