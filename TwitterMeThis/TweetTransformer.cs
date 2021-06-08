using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace TwitterMeThis
{
    public static class TweetTransformer
    {
        public static IEnumerable<Tweet> TwitterResponseToDesignatedTweet( IEnumerable<TwitterResponse> responses)
        {
            var recordDate = DateTime.UtcNow;
            var tweets = responses.Select(x => new Tweet()
                {
                    CreatedAt = recordDate.ToString(),
                    Id = x.Id_Str,
                    Text = x.Text,
                    ScreenName = x.User.Screen_Name,
                    IsPositive = (x.Possibly_Sensitive || DetermineIfIsPositive(x.Text)) ? false : true //Dont neet to explicily say true or false
                }
            );
            return tweets;
        }

        public static bool DetermineIfIsPositive(string text)
        {
            var negativeIndicators = new string[]{
                "politics",
                "politic",
                "democrats",
                "democrat",
                "china",
                "protests",
                "protest",
                "guns",
                "gun",
                "speech",
                "liberal",
                "govenors",
                "mayors",
                "media"
            };
            //This can just use the boolean
            var foundWords = new List<string>();
            foreach( var indicator in negativeIndicators)
            {
                if (text.Contains(indicator))
                {
                    foundWords.Add(indicator);
                };

            }

            return foundWords.Any();
        }
    }
}