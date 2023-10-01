using Microsoft.IdentityModel.Tokens;
using MK.Domain.Common;
using MK.Domain.Dto.Response.User;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace MK.Service.Service
{
    public class JwtService : ITokenService
    {
        private SigningCredentials? _credentials;
        private SymmetricSecurityKey? _securityKey;

        private void SetupCredentials()
        {
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfig.JwtSetting.IssuerSigningKey));
            _credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
        }
        public string GetToken(UserResponse user)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfig.JwtSetting.IssuerSigningKey));
            var _TokenExpiryTimeInHour = Convert.ToInt64(AppConfig.JwtSetting.ValidateLifetime);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = AppConfig.JwtSetting.ValidIssuer,
                Audience = AppConfig.JwtSetting.ValidAudience,
                //Expires = DateTime.UtcNow.AddHours(_TokenExpiryTimeInHour),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.RoleName),
                })
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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
