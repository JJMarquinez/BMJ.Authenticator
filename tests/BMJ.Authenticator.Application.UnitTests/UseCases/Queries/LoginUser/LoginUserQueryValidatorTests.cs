using BMJ.Authenticator.Application.UseCases.Users.Queries.LoginUser;
using FluentValidation;

namespace BMJ.Authenticator.Application.UnitTests.UseCases.Queries.LoginUser;

public class LoginUserQueryValidatorTests
{
    [Fact]
    public async void ShouldValidateLoginUserQueryRequest()
    {
        AbstractValidator<LoginUserQuery> validator = new LoginUserQueryValidator();
        var query = new LoginUserQuery
        {
            UserName = "Dan",
            Password = "O8p1w1aI0&c@"
        };
        var token = new CancellationTokenSource().Token;

        var result = await validator.ValidateAsync(query, token);
        Assert.True(result.IsValid);
    }

    [Fact]
    public async void ShouldNotValidateLoginUserQueryGivenNullUserNameAndPassword()
    {
        AbstractValidator<LoginUserQuery> validator = new LoginUserQueryValidator();
        var query = new LoginUserQuery();
        var token = new CancellationTokenSource().Token;

        var result = await validator.ValidateAsync(query, token);
        Assert.False(result.IsValid);
    }

    [Fact]
    public async void ShouldNotValidateLoginUserQueryGivenNullPassword()
    {
        AbstractValidator<LoginUserQuery> validator = new LoginUserQueryValidator();
        var query = new LoginUserQuery
        {
            UserName = "Dan"
        };
        var token = new CancellationTokenSource().Token;

        var result = await validator.ValidateAsync(query, token);
        Assert.False(result.IsValid);
    }

    [Fact]
    public async void ShouldNotValidateLoginUserQueryGivenNullUsername()
    {
        AbstractValidator<LoginUserQuery> validator = new LoginUserQueryValidator();
        var query = new LoginUserQuery
        {
            Password = "O8p1w1aI0&c@"
        };
        var token = new CancellationTokenSource().Token;

        var result = await validator.ValidateAsync(query, token);
        Assert.False(result.IsValid);
    }
}
