using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwitterMeThis
{   
    public class TwitterClient 
    {
        private HttpClient client;
        private ITokenProvider tokenProvider;
        private string screen_name;
        private string exclude_replies = "true";
        private string count = "1";

        public TwitterClient(string hostname, ITokenProvider tokenProvider, string screen_name)
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(hostname + "/1.1/statuses/user_timeline.json"); 
            this.tokenProvider = tokenProvider;
            this.screen_name = screen_name;

        }

        public async Task<IEnumerable<TwitterResponse>> CollectTweets()
        {
            var request = BuildHttpRequestMessage();
            request.Headers.Authorization = new AuthenticationHeaderValue($"Bearer", $"{await tokenProvider.GetToken()}");


            var response = await client.SendAsync(request);
            Console.WriteLine(request);

            var body = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Tweet request Exception: " +
                    $"Status {response.StatusCode}, Reason {response.ReasonPhrase}, Body {body}"
                );
            }
            var twitterResponses = JArray.Parse(body);
            var tweets = twitterResponses.Select( t => t.ToObject<TwitterResponse>());
            return tweets;
            
        }

        private  HttpRequestMessage BuildHttpRequestMessage()
        {
            var queryString = new Dictionary<string, string>()
            {
                {"screen_name", $"{screen_name}"},
                {"exclude_replies", $"{exclude_replies}"},
                {"count", $"{count}"},

            };
            var url = QueryHelpers.AddQueryString(client.BaseAddress.ToString(), queryString);

            return new HttpRequestMessage()
            {
                Method = new HttpMethod("Get"),
                RequestUri = new Uri(url)
            };   
        }
       

    
    }
}