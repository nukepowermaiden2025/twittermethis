using System;

namespace Models
{
    public class TwitterResponse
    {
        public string Created_At { get; set; }
        public string Id_Str { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public bool Is_Quote_Status { get; set; }
        public int Retweet_Count { get; set; }
        public int Favorite_Count { get; set; }
        public bool Favorited { get; set; }
        public bool Retweeted { get; set; }
        public bool Possibly_Sensitive {get; set;}
    }

    public class User
    {
        public string Screen_Name { get; set; }
    }
}
