using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using PantrySaver.Models;

namespace PantrySaverAPIPortal.Services.AuthenticationServices
{
    public class AccessTokenManager : IAccessTokenManager
    {
        private readonly IDistributedCache accessTokensCache;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration _config;
        public AccessTokenManager(IConfiguration config, IDistributedCache distributedCache, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            accessTokensCache = distributedCache;
            this.httpContextAccessor = httpContextAccessor;
        }
        public string GenerateToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = Encoding.ASCII.GetBytes(_config["Token:Key"]);

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken accessToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(accessToken);
        }

        public async Task<bool> IsActiveAsync(string token)
        {
            return await accessTokensCache.GetStringAsync(GetKey(token)) == null;
        }

        public async Task<bool> IsCurrentActiveToken() => await IsActiveAsync(GetCurrentAsync());

        private string GetCurrentAsync()
        {
            var authorizationHeader = httpContextAccessor.HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty ? string.Empty : authorizationHeader.Single().Split(" ").Last();
        }

        private static string GetKey(string token) => $"tokens:{token}:deactivated";
    }
}