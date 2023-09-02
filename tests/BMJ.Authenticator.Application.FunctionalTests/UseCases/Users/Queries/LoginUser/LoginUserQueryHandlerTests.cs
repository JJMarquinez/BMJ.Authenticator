using BMJ.Authenticator.Adapter.Common;
using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Application.FunctionalTests.TestContext;
using BMJ.Authenticator.Application.UseCases.Users.Queries.LoginUser;

namespace BMJ.Authenticator.Application.FunctionalTests.UseCases.Users.Queries.LoginUser;

[Collection(nameof(AuthenticatorTestConextCollection))]
public class LoginUserQueryHandlerTests : IAsyncLifetime
{
    private readonly AuthenticatorTestConext _testContext;
    private readonly Func<Task> _resetDatabase;

    public LoginUserQueryHandlerTests(AuthenticatorTestConext testContext)
    {
        _testContext = testContext;
        _resetDatabase = _testContext.ResetState;
    }

    public Task DisposeAsync() => _resetDatabase();

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ShouldGetToken()
    {
        var userDto = new UserDto
        {
            UserName = "Joe",
            Email = "joe@authenticator.com",
            PhoneNumber = "111-444-777",
            Roles = new[] { "Guest" }
        };
        await _testContext.AddAsync(userDto, "kN$l2£1q59Y?");

        var query = new LoginUserQuery
        { 
            UserName = "Joe",
            Password = "kN$l2£1q59Y?"
        };
        var result = await _testContext.SendAsync(query);

        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.False(string.IsNullOrEmpty(result.Value!));
    }

    [Fact]
    public async Task ShouldNotGetToken()
    {
        var error = InfrastructureError.Identity.UserNameOrPasswordNotValid;
        var query = new LoginUserQuery
        {
            UserName = "Megan",
            Password = ">+$93p]!5£Ki"
        };
        var result = await _testContext.SendAsync(query);

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(error.Title, result.Error.Title);
        Assert.Equal(error.Detail, result.Error.Detail);
        Assert.Equal(error.Code, result.Error.Code);
        Assert.Equal(error.HttpStatusCode, result.Error.HttpStatusCode);
    }
}
