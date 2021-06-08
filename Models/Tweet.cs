using System;

namespace Models
{
    public class Tweet
    {
        public string CreatedAt { get; set; }
        public string Id { get; set; }
        public string Text { get; set; }
        public string ScreenName { get; set; }
        public bool IsPositive { get; set; }
    }
}