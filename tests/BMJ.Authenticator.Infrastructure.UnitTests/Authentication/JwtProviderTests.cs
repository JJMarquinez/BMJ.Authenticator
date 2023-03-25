using BMJ.Authenticator.Domain.Entities.Users;
using BMJ.Authenticator.Domain.ValueObjects;
using BMJ.Authenticator.Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace BMJ.Authenticator.Infrastructure.UnitTests.Authentication
{
    public class JwtProviderTests
    {
        JwtOptions _jwtOptions = null!;
        JwtProvider _jwtProvider = null!;
        User _user = null!;
        public JwtProviderTests()
        {
            _jwtOptions = new JwtOptions
            {
                SecretKey = "03gno14wOJ#jSmZ4@VZmou!^5tMX$UyieyMZSuRA",
                Audience = "http://localhost",
                Issuer = "http://localhost"
            };
            _jwtProvider = new(Options.Create(_jwtOptions));
            _user = User.New(
                Guid.NewGuid().ToString(),
                "Jaden",
                Email.From("jaden@authenticator.com"),
                new[] { "Standard" },
                Phone.New("111-222-3333"),
                Guid.NewGuid().ToString());
        }

        [Fact]
        public void ShouldGenerateToken()
        {
            string token = _jwtProvider.Generate(_user);
            Assert.NotNull(token);  
            Assert.NotEmpty(token);
        }
    }
}