using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Validation
{
    public class PasswordConfirmPasswordSameValidationAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var user = validationContext.ObjectInstance as User;
            if (!user.ValidationPasswordConfirmPasswordAreSame())
                return new ValidationResult("Password and Confirm passowrd should be same");
            return ValidationResult.Success;
        }
    }
}
