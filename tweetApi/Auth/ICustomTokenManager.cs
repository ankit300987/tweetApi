using System.Threading.Tasks;

namespace tweetApi.Auth
{
    public interface ICustomTokenManager
    {
        Task<string> CreateTokenAsync(string userName);
        Task<string> GetUserInfoFromTokenAsync(string token);
        Task<bool> VerifyTokenAsync(string token);
    }
}