using BMJ.Authenticator.Adapter.Common;
using BMJ.Authenticator.Application.FunctionalTests.TestContext;
using BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser;

namespace BMJ.Authenticator.Application.FunctionalTests.UseCases.Users.Commands.CreateUser;

[Collection(nameof(AuthenticatorTestConextCollection))]
public class CreateUserCommandHandlerTests : IAsyncLifetime
{
    private readonly AuthenticatorTestConext _testContext;
    private readonly Func<Task> _resetDatabase;

    public CreateUserCommandHandlerTests(AuthenticatorTestConext testContext)
    {
        _testContext = testContext;
        _resetDatabase = _testContext.ResetState;
    }

    public Task DisposeAsync() => _resetDatabase();

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ShouldCreateUser()
    {
        var command = new CreateUserCommand
        {
            UserName = "Drake",
            Email = "drake@authenticator.com",
            PhoneNumber = "123-456-789",
            Password = "K6#?m412kNSe",
        };

        var result = await _testContext.SendAsync(command);

        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task ShouldNotCreateUserGivenInValidPassword()
    {
        var command = new CreateUserCommand
        {
            UserName = "Drake",
            Email = "drake@authenticator.com",
            PhoneNumber = "123-456-789",
            Password = "1234",
        };

        var result = await _testContext.SendAsync(command);

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task ShouldNotCreateUserGivenInValidUsername()
    {
        var command = new CreateUserCommand
        {
            UserName = "Drake&",
            Email = "drake@authenticator.com",
            PhoneNumber = "123-456-789",
            Password = "K6#?m412kNSe",
        };

        var result = await _testContext.SendAsync(command);

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }
}
