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
using System.Threading;
using Newtonsoft.Json.Linq;

namespace AcceptanceTests
{
    [Binding]
    
    public class CollectTweetsSteps :IClassFixture<MockedServer>
    {

        [Given(@"the twitter login api responds with")]
        public void TheTwitterLoginApiRespondsWith(string tokenJson)
        {
            var auth = "Base64StringWithConsumerKeyAndSecret";
            Hooks.TwitterLoginStub.Given(
                Request.Create()
                    .WithPath($"/oauth2/token")
                    .WithHeader("Authorization",$"Basic {auth}")
                    .WithParam("grant_type", "client_credentials")
                    .WithHeader("Content-Type","application/x-www-form-urlencoded; charset=utf-8")
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
                .WithHeader("Authorization", "a superlong token")
                .UsingGet()
                )
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
        
        [Then(@"I expect a file named twitter.json to have")]
        public void ThenIExpectMyJsonFileToHave(string expectedJson)
        {   
            var filePath = "//Users//hidigitalkourt//kata//twittermethis//twitter.json";
            
            for (int count = 0; count < 20 && !File.Exists(filePath); count++)
            {
                Thread.Sleep(1000);
            }

            var actual = new List<Tweet>();
            using (StreamReader file = new StreamReader(filePath))
            {
                string json = file.ReadToEnd();
                actual.AddRange(JsonConvert.DeserializeObject<List<Tweet>>(json));
            }
            var expected = JsonConvert.DeserializeObject<List<Tweet>>(expectedJson);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}