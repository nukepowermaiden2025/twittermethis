using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TwitterMeThis
{
    public class TokenProvider
    {
        private string hostname;
        private string consumerKey;
        private string consumerSecret;
        private string auth;
        private HttpClient client;
        public TokenProvider(string hostname, string auth)
        {
            this.hostname = hostname;
            this.auth = ConvertKeyAndSecretToBase64String();
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(hostname + "/oauth2/token");
        }

        public async Task<string> GetToken()
        {
            var request = new HttpRequestMessage();
            return "";
        }

        private string ConvertKeyAndSecretToBase64String()
        {
            var textBytes = Encoding.UTF8.GetBytes($"{consumerKey}:{consumerSecret}");
            return Convert.ToBase64String(textBytes);
        }

    }
}