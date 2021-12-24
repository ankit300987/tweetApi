using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using tweetApi.Auth;

namespace tweetApi.Filters
{
    public class TokenAuthFilterAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string tokenHeader = "X-Token";
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(!context.HttpContext.Request.Headers.TryGetValue(tokenHeader, out var token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var tokenManager = context.HttpContext.RequestServices.GetService(typeof(ICustomTokenManager)) as ICustomTokenManager;
            if(tokenManager == null || !tokenManager.VerifyTokenAsync(token).Result)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

        }

    }
}
