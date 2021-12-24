using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using tweetApi.Auth;

namespace tweetApi.Controllers.Auth
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ICustomTokenManager customTokenManager;
        public AuthController(ICustomTokenManager customTokenManager)
        {
            this.customTokenManager = customTokenManager;
        }


        [HttpGet]
        [Route("api/CreateToken/{clientId}")]
        public async Task<string> CreateToken(string clientId)
        {
            return await customTokenManager.CreateTokenAsync(clientId);
        }

        [HttpGet]
        [Route("api/VerifyToken")]
        public async Task<bool> VerifyTokenAsync(string token)
        {
            return await customTokenManager.VerifyTokenAsync(token);
        }
        [HttpGet]
        [Route("api/GetUserInfo")]
        public async Task<string> GetUserInfoByTokenAsync(string token)
        {
            return await customTokenManager.GetUserInfoFromTokenAsync(token);
        }
    }
}
