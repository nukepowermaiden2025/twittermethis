using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;

namespace TwitterMeThis
{
    public class TokenProvider 
    {
        private string consumerKey;
        private string consumerSecret;
        private HttpClient client;
        
        public TokenProvider(string hostname, string consumerKey, string consumerSecret)
        {
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(hostname + "/oauth2/token"); 
        }

        public async Task<string> GetToken()
        {
            var url = QueryHelpers.AddQueryString(client.BaseAddress.ToString(), "grant_type", "client_credentials");
            var authorization = ConvertKeyAndSecretToBase64String();
            var request = new HttpRequestMessage()
            {
                Content = new StringContent("{}", UnicodeEncoding.UTF8, "application/x-www-form-urlencoded"),
                Method = new HttpMethod("Post"),
                RequestUri = new Uri(url)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue($"Basic", $"{authorization}");

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException("Token Request Exception: " +
                    $"Status {response.StatusCode}, Reason {response.ReasonPhrase}, Body {body}");
            }
            else
            {
                var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                return responseJson["access_token"].ToString();
            } 
        }

        private string ConvertKeyAndSecretToBase64String()
        {
            var textBytes = Encoding.UTF8.GetBytes($"{consumerKey}:{consumerSecret}");
            return Convert.ToBase64String(textBytes);
        }

        

    }
}