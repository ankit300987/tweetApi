using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.Models
{
    public class Tweet
    {
        [JsonIgnore]
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
        public IEnumerable<User> LikesBy { get; set; }
    }
}
