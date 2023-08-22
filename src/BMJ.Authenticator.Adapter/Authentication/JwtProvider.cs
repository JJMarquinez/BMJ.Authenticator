using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BMJ.Authenticator.Adapter.Authentication
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;

        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public string Generate(UserDto user)
        {
            var claims = new List<Claim> {
                new (JwtRegisteredClaimNames.Sub, user.Id),
                new (JwtRegisteredClaimNames.Name, user.UserName),
                new (JwtRegisteredClaimNames.Email, user.Email)
            };
            
            foreach (string role in user.Roles ?? Array.Empty<string>())
                claims.Add(new Claim(ClaimTypes.Role, role));
            
            
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_options.SecretKey)
                ),
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                null,
                DateTime.UtcNow.AddMinutes(15),
                signingCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
