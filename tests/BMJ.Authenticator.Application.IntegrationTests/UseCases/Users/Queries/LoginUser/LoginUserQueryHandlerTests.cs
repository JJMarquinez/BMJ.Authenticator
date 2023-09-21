using BMJ.Authenticator.Application.FunctionalTests.TestContext;
using BMJ.Authenticator.Application.UseCases.Users.Queries.LoginUser;
using BMJ.Authenticator.Infrastructure.Identity;

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
        var applicationUser = ApplicationUser.Builder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        await _testContext.AddAsync(applicationUser, "kN$l2£1q59Y?", roles);

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
    public async Task ShouldNotGetTokenGivenInvalidCredetials()
    {
        var query = new LoginUserQuery
        {
            UserName = "Megan",
            Password = ">+$93p]!5£Ki"
        };
        var result = await _testContext.SendAsync(query);

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task ShouldNotGetTokenGivenNoPassword()
    {
        var query = new LoginUserQuery
        {
            UserName = "Megan"
        };
        var result = await _testContext.SendAsync(query);

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.NotNull(result.Error);

    }

    [Fact]
    public async Task ShouldThrowArgumentNullExceptionGivenNoUserName()
    {
        var query = new LoginUserQuery
        {
            Password = ">+$93p]!5£Ki"
        };
        await Assert.ThrowsAsync<ArgumentNullException>(() => _testContext.SendAsync(query));
    }
}
