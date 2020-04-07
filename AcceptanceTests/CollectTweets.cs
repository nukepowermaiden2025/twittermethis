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

namespace AcceptanceTests
{
    [Binding]
    
    public class CollectTweetsSteps
    {
        //Need a place to store the twitter results
        public StreamReader streamreader;
        [Given(@"the twitter api responds with")]
        public void GivenTheTwitterApiRespondsWith(string tweetsJson)
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

        [When(@"I trigger the function CollectTweets")]
        public void WhenITriggerTheFunctionCollectTweets()
        {
            var tweets = new TwitterClient();
            //This is the place that the prod code will be tested
        }

        [Then(@"I expect my json file to have")]
        public void ThenIExpectMyJsonFileToHave(string expectedJson)
        {
            var expected = JsonConvert.DeserializeObject<IEnumerable<Tweet>>(expectedJson);
            var actualTweets = TweetTransformer.TwitterResponseToDesignatedTweet();

            actualTweets.Should().BeEquivalentTo(expected);

        }
    }
}