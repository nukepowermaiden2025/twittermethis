using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using InvoicingTests;
using Models;
using Moq;
using Newtonsoft.Json;
using TwitterMeThis;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace TwitterMeThisTests
{
    public class TwitterClientTests :IClassFixture<MockedServer>
    {
        private ITokenProvider mockTokenProvider; 
        private FluentMockServer server;
        private string hostname;
        
        public TwitterClientTests(MockedServer mockserver)
        {
            this.server = mockserver.MockServer;
            this.hostname = server.Urls.FirstOrDefault();
            var mockToken = new Mock<ITokenProvider>();
            mockToken.Setup( tp => tp.GetToken()).Returns(Task.FromResult("token"));
            mockTokenProvider = mockToken.Object; 
        }

        [Fact]
        public async Task GetsTweets()
        {
            var screenName = "supercooluser";
            var excludeReplies = true;
            var count = "1";
            
            server.Given(
                Request.Create()
                    .WithPath("/1.1/statuses/user_timeline.json")
                    .WithParam("screen_name", screenName)
                    .WithParam("exclude_replies", excludeReplies)
                    .WithParam("count", count)
                    .UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBody(@"{[
                        {
                            'created_at': 'Thu Apr 06 15:28:43 +0000 2020',
                            'id_str': '850007368138018817',
                            'text': 'RT @TwitterDev: 1/ Today we’re sharing our plan for the future',
                            'user': {
                                'id_str': '6253282',
                                'screen_name': 'twitterapi'
                                },
                            'is_quote_status': false,
                            'retweet_count': 284,
                            'favorite_count': 0,
                            'favorited': false,
                            'retweeted': false,
                            'possibly_sensitive': true
                        }
                    ]}"
                )    
            );

            var expected = new List<TwitterResponse>()
            {
                new TwitterResponse()
                {
                    CreatedAt = new DateTime(2020, 04, 06, 15, 28, 43, DateTimeKind.Utc),
                    Id_Str = "850007368138018817",
                    TweetText = "RT @TwitterDev: 1/ Today we’re sharing our plan for the future",
                    User = new User() 
                        {
                        Screen_Name = "twitterapi"
                        },
                    Is_Quote_Status = false,
                    Retweet_Count = 284,
                    Favorite_Count = 0,
                    Retweeted = false,
                    Favorited = false,
                    Possibly_Sensative = true
                }
            };

            var client = new TwitterClient(hostname, mockTokenProvider);
            var actual = await client.CollectTweets();

            var allReqs = server.LogEntries;
            var jsonLogs = JsonConvert.SerializeObject(allReqs, Formatting.Indented);
            Console.WriteLine(jsonLogs);

            actual.Should().BeEquivalentTo(expected);


        }

    }
}