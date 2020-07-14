using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwitterMeThis
{
    public class GetTweets
    {
       public async Task RunAsync()
       {
           var loginHostname= Environment.GetEnvironmentVariable("TWITTER_LOGIN_HOSTNAME"); 
           var consumerKey = Environment.GetEnvironmentVariable("CONSUMER_KEY");
           var consumerSecret = Environment.GetEnvironmentVariable("CONSUMER_SECRET");
           var apihostname = Environment.GetEnvironmentVariable("TWITTER_CLIENT_HOSTNAME");
           var screenName = Environment.GetEnvironmentVariable("SCREEN_NAME");
           
           var tokenProvider = new TokenProvider(loginHostname, consumerKey, consumerSecret);
           var client = new TwitterClient(apihostname, tokenProvider, screenName);

           var twitterResponses =  await client.CollectTweets();
           var tweets = TweetTransformer.TwitterResponseToDesignatedTweet(twitterResponses);

           using( StreamWriter file = File.CreateText(@"tweet.json"))
           {
               var serializer = new JsonSerializer();
               serializer.Serialize(file, tweets);
           }
       }
    }
}