using BMJ.Authenticator.Domain.Entities.Users;
using BMJ.Authenticator.Domain.ValueObjects;
using BMJ.Authenticator.Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BMJ.Authenticator.Infrastructure.UnitTests.Authentication
{
    public class JwtProviderTests
    {
        [Fact]
        public void ShouldGenerateToken()
        {
            JwtOptions jwtOptions = new JwtOptions
            {
                SecretKey = "03gno14wOJ#jSmZ4@VZmou!^5tMX$UyieyMZSuRA",
                Audience = "http://localhost",
                Issuer = "http://localhost"
            };
            JwtProvider jwtProvider = new(Options.Create(jwtOptions));
            User user = User.New(
                Guid.NewGuid().ToString(),
                "Jaden",
                Email.From("jaden@authenticator.com"),
                new[] { "Standard" },
                Phone.New("111-222-3333"),
                Guid.NewGuid().ToString());

            string token = jwtProvider.Generate(user);

            Assert.NotNull(token);  
            Assert.NotEmpty(token);
        }

        [Fact]
        public void ShoulNotdGenerateTokenGivenSecretKeyWithLenthLessThen128bit()
        {
            JwtOptions jwtOptions = new JwtOptions
            {
                SecretKey = "nO7*!%xGX59!1nM",
                Audience = "http://localhost",
                Issuer = "http://localhost"
            };
            JwtProvider jwtProvider = new(Options.Create(jwtOptions));
            User user = User.New(
                Guid.NewGuid().ToString(),
                "Jaden",
                Email.From("jaden@authenticator.com"),
                new[] { "Standard" },
                Phone.New("111-222-3333"),
                Guid.NewGuid().ToString());

            Assert.Throws<ArgumentOutOfRangeException>(() => jwtProvider.Generate(user));
        }

        [Fact]
        public void ShouldNotGenerateTokenGivenNullSecretKey()
        {
            JwtOptions jwtOptions = new JwtOptions
            {
                SecretKey = null!,
                Audience = "http://localhost",
                Issuer = "http://localhost"
            };
            JwtProvider jwtProvider = new(Options.Create(jwtOptions));
            User user = User.New(
                Guid.NewGuid().ToString(),
                "Jaden",
                Email.From("jaden@authenticator.com"),
                new[] { "Standard" },
                Phone.New("111-222-3333"),
                Guid.NewGuid().ToString());

            Assert.Throws<ArgumentNullException>(() => jwtProvider.Generate(user));
        }

        [Fact]
        public void ShouldTokenHasTheUserDataAsClaims()
        {
            JwtOptions jwtOptions = new JwtOptions
            {
                SecretKey = "03gno14wOJ#jSmZ4@VZmou!^5tMX$UyieyMZSuRA",
                Audience = "http://localhost",
                Issuer = "http://localhost"
            };
            JwtProvider jwtProvider = new(Options.Create(jwtOptions));
            User user = User.New(
                Guid.NewGuid().ToString(),
                "Jaden",
                Email.From("jaden@authenticator.com"),
                new[] { "Standard" },
                Phone.New("111-222-3333"),
                Guid.NewGuid().ToString());

            string token = jwtProvider.Generate(user);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(jwtEncodedString: token);

            Assert.Equal(user.GetId(), jwtSecurityToken.Claims.First(claim => string.Equals(claim.Type, JwtRegisteredClaimNames.Sub, StringComparison.Ordinal)).Value);
            Assert.Equal(user.GetUserName(), jwtSecurityToken.Claims.First(claim => string.Equals(claim.Type, JwtRegisteredClaimNames.Name, StringComparison.Ordinal)).Value);
            Assert.Equal(user.GetEmail(), jwtSecurityToken.Claims.First(claim => string.Equals(claim.Type, JwtRegisteredClaimNames.Email, StringComparison.Ordinal)).Value);

            //if(user.GetRoles() != null)
            //{
            //    Assert.Equals(user.GetRoles(), jwtSecurityToken.Claims.First(claim => string.Equals(claim.Type, ClaimTypes.Role, StringComparison.Ordinal)).Value);
            //}
        }
    }
}