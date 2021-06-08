using System;
using System.Collections.Generic;
using FluentAssertions;
using Models;
using TwitterMeThis;
using Xunit;

namespace TwitterMeThisTests
{
    public class TweetTransformerTests
    {
        [Fact]
        public void TransformsTwitterResponsesIntoTweetsForPositiveTweets()
        {
            var id = "850007368138018817";
            var text = "RT @TwitterDev: 1/ Today we’re sharing our plan for the future";
            var screenName = "twitterapi";

            var inputs = new List<TwitterResponse>()
            {
                new TwitterResponse()
                {
                    Created_At = "Thu Apr 06 15:28:43 +0000 2020",
                    Id_Str = id,
                    Text = text,
                    User = new User() 
                        {
                        Screen_Name = screenName
                        },
                    Is_Quote_Status = false,
                    Retweet_Count = 284,
                    Favorite_Count = 0,
                    Retweeted = false,
                    Favorited = false,
                    Possibly_Sensitive = false
                }
            }; 

            var recordDate = DateTime.UtcNow;

            var expected = new List<Tweet>()
            {
                new Tweet()
                {
                    CreatedAt = recordDate.ToString(),
                    Id = id,
                    Text = text,
                    ScreenName = screenName,
                    IsPositive = true
                }
            };

            var actual = TweetTransformer.TwitterResponseToDesignatedTweet(inputs);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void TransformsTwitterResponsesIntoTweetsForNegativeTweetsDueToSensitivy()
        {
            var id = "850007368138018817";
            var text = "RT @TwitterDev: 1/ Today we’re sharing our plan";
            var screenName = "twitterapi";

            var inputs = new List<TwitterResponse>()
            {
                new TwitterResponse()
                {
                    Created_At = "Thu Apr 06 15:28:43 +0000 2020",
                    Id_Str = id,
                    Text = text,
                    User = new User() 
                        {
                        Screen_Name = screenName
                        },
                    Is_Quote_Status = false,
                    Retweet_Count = 284,
                    Favorite_Count = 0,
                    Retweeted = false,
                    Favorited = false,
                    Possibly_Sensitive = true
                }
            }; 

            var recordDate = DateTime.UtcNow;

            var expected = new List<Tweet>()
            {
                new Tweet()
                {
                    CreatedAt = recordDate.ToString(),
                    Id = id,
                    Text = text,
                    ScreenName = screenName,
                    IsPositive = false
                }
            };

            var actual = TweetTransformer.TwitterResponseToDesignatedTweet(inputs);
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void TransformsTwitterResponsesIntoTweetsForNegativeTweetsDueToNegativeWords()
        {
            var id = "850007368138018817";
            var text = "RT @TwitterDev: 1/ Today we’re sharing our plan to get back at china";
            var screenName = "twitterapi";

            var inputs = new List<TwitterResponse>()
            {
                new TwitterResponse()
                {
                    Created_At = "Thu Apr 06 15:28:43 +0000 2020",
                    Id_Str = id,
                    Text = text,
                    User = new User() 
                        {
                        Screen_Name = screenName
                        },
                    Is_Quote_Status = false,
                    Retweet_Count = 284,
                    Favorite_Count = 0,
                    Retweeted = false,
                    Favorited = false,
                    Possibly_Sensitive = false
                }
            }; 

            var recordDate = DateTime.UtcNow;

            var expected = new List<Tweet>()
            {
                new Tweet()
                {
                    CreatedAt = recordDate.ToString(),
                    Id = id,
                    Text = text,
                    ScreenName = screenName,
                    IsPositive = false
                }
            };

            var actual = TweetTransformer.TwitterResponseToDesignatedTweet(inputs);
            actual.Should().BeEquivalentTo(expected);
        }
    }
}