using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using InvoicingTests;
using Newtonsoft.Json;
using TwitterMeThis;
using WireMock.Logging;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace TwitterMeThisTests
{
    public class TokenProviderTests : IClassFixture<MockedServer>
    {
        const string consumerKey = "twittermethisappKey"; 
        const string consumerSecret = "AAAAASUPERCOOLSECRET123";
        private FluentMockServer server;
        private string hostname;
        public TokenProviderTests(MockedServer mockserver)
        {
            this.server = mockserver.MockServer;
            this.hostname = server.Urls.FirstOrDefault();
        }
        [Fact]
        public async Task GetsLoginToken()
        {
           
            var textBytes = Encoding.UTF8.GetBytes($"{consumerKey}:{consumerSecret}");
            var auth = Convert.ToBase64String(textBytes);

            server.Given(
                Request.Create()
                    .WithPath($"/oauth2/token")
                    .WithHeader("Authorization",$"Basic {auth}")
                    .WithHeader("Content-Type","application/x-www-form-urlencoded; charset=UTF-8")
                    .UsingPost()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type","application/json; charset=utf-8")
                    .WithHeader("Content-Encoding","gzip")
                    .WithHeader("Content-Length","140")
                    .WithBody(@"{'access_token':'here is your login token for twitter'}")      
            );
            
          
            var tokenProvider = new TokenProvider(hostname, consumerKey, consumerSecret);
            var token = await tokenProvider.GetToken();

            var allReqs = server.LogEntries;
            var jsonLogs = JsonConvert.SerializeObject(allReqs, Formatting.Indented);
            Console.WriteLine(jsonLogs);
            Console.WriteLine($"There are a total of {server.LogEntries.Count()} Logs");
            server.DeleteLogEntry(new Guid("5f14361f-b864-4159-b222-86df47cdbf2d"));
            Console.WriteLine("The Logs have been deleted ",jsonLogs);

            token.Should().Be("here is your login token for twitter");
             
        }
        

    }
}