using System.Net.Http;

namespace TwitterMeThis
{   
    public class TwitterClient
    {
        private HttpClient client;

        public TwitterClient()
        {
            this.client = new HttpClient();
        }
    }
}