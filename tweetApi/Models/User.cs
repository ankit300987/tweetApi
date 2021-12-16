using System.ComponentModel.DataAnnotations;

namespace tweetApi
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string EmailId { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Image { get; set; }
    }
}