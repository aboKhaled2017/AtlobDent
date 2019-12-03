using Atlob_Dent.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Atlob_Dent
{
    public class SecurityHelper
    {
        private static readonly IConfigurationSection _JWT = GlobalProperties.configuration.GetSection("JWT");
        public static JwtSecurityToken GetJWTSecurityToken(string userName)
        {
            return GetJWTSecurityToken(userName, claims => { });
        }
        public static TokenResponse GetTokenResponse(string userName)
        {
            return GetTokenResponse(userName,c=> { });
        }
        public static TokenResponse GetTokenResponse(string userName, Action<Claim[]> addClaims)
        {
            var token = GetJWTSecurityToken(userName,addClaims);
            return new TokenResponse
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expireAt = token.ValidTo.ToString()
            };
        }
        public static JwtSecurityToken GetJWTSecurityToken(string userName, Action<Claim[]> addClaims)
        {
            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,userName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JWT.GetValue<string>("signingKey")));
            addClaims(claims);
                return new JwtSecurityToken(
                    issuer: _JWT.GetValue<string>("issuer"),
                    expires: DateTime.UtcNow.AddYears(10),
                    audience: _JWT.GetValue<string>("audience"),
                    claims: claims,
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                    );
        }
        public static string GetTokenStr(JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
