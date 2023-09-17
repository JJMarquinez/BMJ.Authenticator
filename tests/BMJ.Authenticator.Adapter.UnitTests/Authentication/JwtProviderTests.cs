using BMJ.Authenticator.Adapter.Authentication;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Domain.ValueObjects;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BMJ.Authenticator.Adapter.UnitTests.Authentication;

public class JwtProviderTests
{
    [Fact]
    public async Task ShouldGenerateToken()
    {
        JwtOptions jwtOptions = JwtOptions.Builder()
            .WithSecretKey("03gno14wOJ#jSmZ4@VZmou!^5tMX$UyieyMZSuRA")
            .WithAudience("http://localhost")
            .WithIssuer("http://localhost")
            .Build();
        IJwtProvider jwtProvider = new JwtProvider(Options.Create(jwtOptions));

        UserDto user = new UserDto
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Jaden",
            Email = Email.From("jaden@authenticator.com"),
            Roles = new[] { "Standard" },
            PhoneNumber = Phone.New("111-222-3333"),
        };

        string token = await jwtProvider.GenerateAsync(user);

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
        IJwtProvider jwtProvider = new JwtProvider(Options.Create(jwtOptions));

        UserDto user = new UserDto
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Jaden",
            Email = Email.From("jaden@authenticator.com"),
            Roles = new[] { "Standard" },
            PhoneNumber = Phone.New("111-222-3333"),
        };

        Assert.Throws<ArgumentOutOfRangeException>(() => jwtProvider.GenerateAsync(user));
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullSecretKey()
    {
        JwtOptions jwtOptions = JwtOptions.Builder()
            .WithAudience("http://localhost")
            .WithIssuer("http://localhost")
            .Build();

        IJwtProvider jwtProvider = new JwtProvider(Options.Create(jwtOptions));

        UserDto user = new UserDto
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Jaden",
            Email = Email.From("jaden@authenticator.com"),
            Roles = new[] { "Standard" },
            PhoneNumber = Phone.New("111-222-3333"),
        };

        Assert.Throws<ArgumentNullException>(() => jwtProvider.GenerateAsync(user));
    }

    [Fact]
    public async Task ShouldGenarateTokenWithTheUserData()
    {
        JwtOptions jwtOptions = JwtOptions.Builder()
            .WithSecretKey("03gno14wOJ#jSmZ4@VZmou!^5tMX$UyieyMZSuRA")
            .WithAudience("http://localhost")
            .WithIssuer("http://localhost")
            .Build(); ;
        IJwtProvider jwtProvider = new JwtProvider(Options.Create(jwtOptions));

        UserDto user = new UserDto
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Jaden",
            Email = Email.From("jaden@authenticator.com"),
            Roles = new[] { "Standard" },
            PhoneNumber = Phone.New("111-222-3333"),
        };

        string token = await jwtProvider.GenerateAsync(user);
        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(jwtEncodedString: token);

        Assert.Equal(user.Id, jwtSecurityToken.Claims.First(claim => string.Equals(claim.Type, JwtRegisteredClaimNames.Sub, StringComparison.Ordinal)).Value);
        Assert.Equal(user.UserName, jwtSecurityToken.Claims.First(claim => string.Equals(claim.Type, JwtRegisteredClaimNames.Name, StringComparison.Ordinal)).Value);
        Assert.Equal(user.Email, jwtSecurityToken.Claims.First(claim => string.Equals(claim.Type, JwtRegisteredClaimNames.Email, StringComparison.Ordinal)).Value);
        Assert.Equal(user.Roles, jwtSecurityToken.Claims.Where(claim => string.Equals(claim.Type, ClaimTypes.Role, StringComparison.Ordinal)).Select(claim => claim.Value).ToArray());
    }

    [Fact]
    public async Task ShouldGenarateTokenWithTheUserDataExceptFromRoles()
    {
        JwtOptions jwtOptions = JwtOptions.Builder()
            .WithSecretKey("03gno14wOJ#jSmZ4@VZmou!^5tMX$UyieyMZSuRA")
            .WithAudience("http://localhost")
            .WithIssuer("http://localhost")
            .Build();

        IJwtProvider jwtProvider = new JwtProvider(Options.Create(jwtOptions));

        UserDto user = new UserDto
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Jaden",
            Email = Email.From("jaden@authenticator.com"),
            PhoneNumber = Phone.New("111-222-3333"),
        };

        string token = await jwtProvider.GenerateAsync(user);
        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(jwtEncodedString: token);

        Assert.Equal(user.Id, jwtSecurityToken.Claims.First(claim => string.Equals(claim.Type, JwtRegisteredClaimNames.Sub, StringComparison.Ordinal)).Value);
        Assert.Equal(user.UserName, jwtSecurityToken.Claims.First(claim => string.Equals(claim.Type, JwtRegisteredClaimNames.Name, StringComparison.Ordinal)).Value);
        Assert.Equal(user.Email, jwtSecurityToken.Claims.First(claim => string.Equals(claim.Type, JwtRegisteredClaimNames.Email, StringComparison.Ordinal)).Value);
        Assert.DoesNotContain(jwtSecurityToken.Claims, claim => string.Equals(claim.Type, ClaimTypes.Role, StringComparison.Ordinal));
    }
}
