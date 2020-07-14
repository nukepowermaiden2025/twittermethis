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
    public interface ITokenProvider
    {
        Task<string> GetToken();
    }
    public class TokenProvider : ITokenProvider
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
            var response = await client.SendAsync(BuildHttpRquestMessage());
            var body = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Token Request Exception: " +
                    $"Status {response.StatusCode}, Reason {response.ReasonPhrase}, Body {body}");
            }
            var responseJson = JObject.Parse(body);
            return responseJson["access_token"].ToString();
        }

        private HttpRequestMessage BuildHttpRquestMessage()
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
            return request;
        }

        private string ConvertKeyAndSecretToBase64String()
        {
            var textBytes = Encoding.UTF8.GetBytes($"{consumerKey}:{consumerSecret}");
            return Convert.ToBase64String(textBytes);
        }

        

    }
}