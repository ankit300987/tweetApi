using Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Core.Validation
{
    public class Tweet_UserCannotLikeOwnTweetValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var tweet = validationContext.ObjectInstance as Tweet;
            if (!tweet.UserCannotLikeOwnTweet())
                return new ValidationResult("User can not like his own tweet");
            return ValidationResult.Success;

        }
    }
}
