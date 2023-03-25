using BMJ.Authenticator.Domain.Entities.Users;
using BMJ.Authenticator.Domain.ValueObjects;
using BMJ.Authenticator.Infrastructure.Authentication;
using Microsoft.Extensions.Options;

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
    }
}