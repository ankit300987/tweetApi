using Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace Core.Models
{
    public class Tweet
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(144)]
        public string Message { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }
        public IEnumerable<Tweet> Reply { get; set; }
        public int? ParentId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Tweet_UserCannotLikeOwnTweetValidation]
        public IEnumerable<string> LikesBy { get; set; }

        public bool UserCannotLikeOwnTweet()
        {
            return !LikesBy.Where( x=> x == UserName ).Any();
        }
    }
}
