using System;
using System.Collections.Generic;

namespace tweetApi
{
    public class Tweet
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public DateTime TimeStamp { get; set; }
        public IEnumerable<Tweet> Reply { get; set; }
        public int ParentId { get; set; }
        public string UserName { get; set; }
        public IEnumerable<User> LikesBy { get; set; }

    }
}