using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace tweetApi.Auth
{
    public class JWTTokenManager : ICustomTokenManager
    {
        private readonly JwtSecurityTokenHandler tokenHandler;
        private readonly byte[] secretKey;

        public JWTTokenManager(IConfiguration configuration)
        {
            tokenHandler = new JwtSecurityTokenHandler();
            secretKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("JwtSecretKey"));
        }
        public Task<string> CreateTokenAsync(string userName)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                   new Claim[]
                   {
                       new Claim(ClaimTypes.Name, userName)
                   }
                 ),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult( tokenHandler.WriteToken(token));
        }

        public Task<string> GetUserInfoFromTokenAsync(string token)
        {
            if(string.IsNullOrEmpty(token))  return null;
            var jwtToken = tokenHandler.ReadToken(token.Replace("\"", string.Empty)) as JwtSecurityToken;
            var claim = jwtToken.Claims.FirstOrDefault(x => x.Type == "unique_name");
            if (claim != null) return Task.FromResult( claim.Value);
            return null;
        }

        public Task<bool> VerifyTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) return Task.FromResult(false);

            SecurityToken securityToken;

            try
            {
                tokenHandler.ValidateToken(
                token.Replace("\"", string.Empty),
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = System.TimeSpan.Zero
                },
                out securityToken);
            }
            catch (SecurityTokenException)
            {
                return Task.FromResult(false);
            }
            catch (Exception)
            {
                throw;
            }
            return Task.FromResult(securityToken != null);
        }
    }
}
