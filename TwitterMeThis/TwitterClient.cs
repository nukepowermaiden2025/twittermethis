using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Models;

namespace TwitterMeThis
{   
    public class TwitterClient 
    {
        private HttpClient client;

        private ITokenProvider tokenProvider;

        private string hostname;

        public TwitterClient(string hostname, ITokenProvider tokenProvider)
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(hostname + "/1.1/statuses/user_timeline.json"); 
            this.tokenProvider = tokenProvider;

        }

        public async Task<IEnumerable<TwitterResponse>> CollectTweets()
        {
            var token = await tokenProvider.GetToken();
            return null;
        }

       

    
    }
}