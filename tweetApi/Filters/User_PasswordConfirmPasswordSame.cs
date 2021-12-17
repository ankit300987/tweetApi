using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace tweetApi.Filters
{
    public class User_PasswordConfirmPasswordSame : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var user = context.ActionArguments["User"] as User;
            if (user != null && !user.Password.Equals(user.ConfirmPassword))
            {
                context.ModelState.AddModelError("ConfirmPassword", "Password & Confirm Password should be same");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }

        }
    }
}
