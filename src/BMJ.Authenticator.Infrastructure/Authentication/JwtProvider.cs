using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BMJ.Authenticator.Infrastructure.Authentication
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;

        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public string Generate(User user)
        {
            var claims = new List<Claim> {
                new (JwtRegisteredClaimNames.Sub, user.GetId()),
                new (JwtRegisteredClaimNames.Name, user.GetUserName())
            };
            
            foreach (var role in user.GetRoles())
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
