using Core.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.Models
{
    public class User
    {
        [JsonIgnore]
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
        [Required]
        public string Password { get; set; }
        [Required]
        [PasswordConfirmPasswordSameValidation]
        public string ConfirmPassword { get; set; }
        public string Image { get; set; }

        public bool ValidationPasswordConfirmPasswordAreSame()
        {
            return Password == ConfirmPassword;
        }
    }
}
