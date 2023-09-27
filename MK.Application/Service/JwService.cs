using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Service
{
    public class JwtService : ITokenService
    {
        private SigningCredentials _credentials;
        private SymmetricSecurityKey _securityKey;

        private void SetupCredentials()
        {
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfig.JwtSetting.IssuerSigningKey));
            _credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
        }
        public string GetToken(User user)
        {
            var claims = new[] {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            return new JwtSecurityTokenHandler().WriteToken(GenerateTokenByClaims(claims, DateTime.Now.AddMinutes(120)));
        }
        private JwtSecurityToken GenerateTokenByClaims(IEnumerable<Claim> claims, DateTime expires)
        {
            return new JwtSecurityToken(AppConfig.JwtSetting.ValidIssuer,
                AppConfig.JwtSetting.ValidAudience,
                claims,
                expires: expires,
                signingCredentials: _credentials);
        }

        public IEnumerable<Claim> DecodeAndValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                return jwtToken.Claims;
            }
            catch
            {
                return null;
            }
        }
    }
}
