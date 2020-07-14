using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Models;
using System.IO;
using Newtonsoft.Json;
using TwitterMeThis;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using TwitterMeThisTests;

namespace AcceptanceTests
{
    [Binding]
    
    public class CollectTweetsSteps :IClassFixture<MockedServer>
    {
        public StreamReader streamreader;

        [Given(@"the twitter login api responds with")]
        public void TheTwitterLoginApiRespondsWith(string tokenJson)
        {
            var auth = "Base64StringWithConsumerKeyAndSecret";
            Hooks.TwitterStub.Given(
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
                    .WithBody(tokenJson)      
            );
        }        
        [Given(@"the twitter api responds with")]
        public void TheTwitterApiRespondsWith(string tweetsJson)
        {
            Hooks.TwitterStub.Given(
                Request.Create()
                .WithPath("/1.1/statuses/user_timeline.json")
                .WithParam("user_id")
                .WithHeader("Authorization", "Bearer of a token")
                .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(tweetsJson));
        }

        [When(@"I trigger the function GetTweets")]
        public async Task ITriggerTheFunctionGetTweets()
        {
            var getTweets = new GetTweets();
            await getTweets.RunAsync();
            
        }
        
        [Then(@"I expect my json file to have")]
        public void ThenIExpectMyJsonFileToHave(string expectedJson)
        {
            var expected = JsonConvert.DeserializeObject<IEnumerable<Tweet>>(expectedJson);
            //I need to read the file that the previous code generates and compare it the the json that I am submitting

            actualTweets.Should().BeEquivalentTo(expected);

        }
    }
}