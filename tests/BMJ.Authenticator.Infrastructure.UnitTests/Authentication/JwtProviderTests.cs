using BMJ.Authenticator.Domain.Entities.Users;
using BMJ.Authenticator.Domain.ValueObjects;
using BMJ.Authenticator.Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BMJ.Authenticator.Infrastructure.UnitTests.Authentication
{
    public class JwtProviderTests
    {
        [Fact]
        public void ShouldGenerateToken()
        {
            JwtOptions jwtOptions = JwtOptions.Builder()
                .WithSecretKey("03gno14wOJ#jSmZ4@VZmou!^5tMX$UyieyMZSuRA")
                .WithAudience("http://localhost")
                .WithIssuer("http://localhost")
                .Build();
            JwtProvider jwtProvider = new(Options.Create(jwtOptions));
            User user =
                User.Builder()
                .WithId(Guid.NewGuid().ToString())
                .WithName("Jaden")
                .WithEmail(Email.From("jaden@authenticator.com"))
                .WithRoles(new[] { "Standard" })
                .WithPhone(Phone.New("111-222-3333"))
                .WithPasswordHash(Guid.NewGuid().ToString())
                .Build();

            string token = jwtProvider.Generate(user);

            Assert.NotNull(token);  
            Assert.NotEmpty(token);
        }

        [Fact]
        public void ShouldThrowArgumentOutOfRangeExceptionGivenSecretKeyWithLenthLessThen128bit()
        {
            JwtOptions jwtOptions = JwtOptions.Builder()
                .WithSecretKey("nO7*!%xGX59!1nM")
                .WithAudience("http://localhost")
                .WithIssuer("http://localhost")
                .Build(); ;
            JwtProvider jwtProvider = new(Options.Create(jwtOptions));
            User user =
                User.Builder()
                .WithId(Guid.NewGuid().ToString())
                .WithName("Jaden")
                .WithEmail(Email.From("jaden@authenticator.com"))
                .WithRoles(new[] { "Standard" })
                .WithPhone(Phone.New("111-222-3333"))
                .WithPasswordHash(Guid.NewGuid().ToString())
                .Build();

            Assert.Throws<ArgumentOutOfRangeException>(() => jwtProvider.Generate(user));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionGivenNullSecretKey()
        {
            JwtOptions jwtOptions = JwtOptions.Builder()
                .WithAudience("http://localhost")
                .WithIssuer("http://localhost")
                .Build(); ;
            JwtProvider jwtProvider = new(Options.Create(jwtOptions));
            User user =
                User.Builder()
                .WithId(Guid.NewGuid().ToString())
                .WithName("Jaden")
                .WithEmail(Email.From("jaden@authenticator.com"))
                .WithRoles(new[] { "Standard" })
                .WithPhone(Phone.New("111-222-3333"))
                .WithPasswordHash(Guid.NewGuid().ToString())
                .Build();

            Assert.Throws<ArgumentNullException>(() => jwtProvider.Generate(user));
        }

        [Fact]
        public void ShouldGenarateTokenWithTheUserData()
        {
            JwtOptions jwtOptions = JwtOptions.Builder()
                .WithSecretKey("03gno14wOJ#jSmZ4@VZmou!^5tMX$UyieyMZSuRA")
                .WithAudience("http://localhost")
                .WithIssuer("http://localhost")
                .Build(); ;
            JwtProvider jwtProvider = new(Options.Create(jwtOptions));
            User user =
                User.Builder()
                .WithId(Guid.NewGuid().ToString())
                .WithName("Jaden")
                .WithEmail(Email.From("jaden@authenticator.com"))
                .WithRoles(new[] { "Standard" })
                .WithPhone(Phone.New("111-222-3333"))
                .WithPasswordHash(Guid.NewGuid().ToString())
                .Build();

            string token = jwtProvider.Generate(user);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(jwtEncodedString: token);

            Assert.Equal(user.GetId(), jwtSecurityToken.Claims.First(claim => string.Equals(claim.Type, JwtRegisteredClaimNames.Sub, StringComparison.Ordinal)).Value);
            Assert.Equal(user.GetUserName(), jwtSecurityToken.Claims.First(claim => string.Equals(claim.Type, JwtRegisteredClaimNames.Name, StringComparison.Ordinal)).Value);
            Assert.Equal(user.GetEmail(), jwtSecurityToken.Claims.First(claim => string.Equals(claim.Type, JwtRegisteredClaimNames.Email, StringComparison.Ordinal)).Value);
            Assert.Equal(user.GetRoles(), jwtSecurityToken.Claims.Where(claim => string.Equals(claim.Type, ClaimTypes.Role, StringComparison.Ordinal)).Select(claim => claim.Value).ToArray());
        }

        [Fact]
        public void ShouldGenarateTokenWithTheUserDataExceptFromRoles()
        {
            JwtOptions jwtOptions = JwtOptions.Builder()
                .WithSecretKey("03gno14wOJ#jSmZ4@VZmou!^5tMX$UyieyMZSuRA")
                .WithAudience("http://localhost")
                .WithIssuer("http://localhost")
                .Build(); ;
            JwtProvider jwtProvider = new(Options.Create(jwtOptions));
            User user =
                User.Builder()
                .WithId(Guid.NewGuid().ToString())
                .WithName("Jaden")
                .WithEmail(Email.From("jaden@authenticator.com"))
                .WithPhone(Phone.New("111-222-3333"))
                .WithPasswordHash(Guid.NewGuid().ToString())
                .Build();

            string token = jwtProvider.Generate(user);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(jwtEncodedString: token);

            Assert.Equal(user.GetId(), jwtSecurityToken.Claims.First(claim => string.Equals(claim.Type, JwtRegisteredClaimNames.Sub, StringComparison.Ordinal)).Value);
            Assert.Equal(user.GetUserName(), jwtSecurityToken.Claims.First(claim => string.Equals(claim.Type, JwtRegisteredClaimNames.Name, StringComparison.Ordinal)).Value);
            Assert.Equal(user.GetEmail(), jwtSecurityToken.Claims.First(claim => string.Equals(claim.Type, JwtRegisteredClaimNames.Email, StringComparison.Ordinal)).Value);
            Assert.DoesNotContain(jwtSecurityToken.Claims, claim => string.Equals(claim.Type, ClaimTypes.Role, StringComparison.Ordinal));
        }
    }
}