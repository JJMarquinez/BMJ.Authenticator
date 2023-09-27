using BMJ.Authenticator.Application.FunctionalTests.TestContext;

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
        var command = _testContext.GetCreateUserCommandBuilder()
            .WithUsername("Drake")
            .WithEmail("drake@authenticator.com")
            .WithPhoneNumber("123-456-789")
            .WithPassword("K6#?m412kNSe")
            .Build();

        var result = await _testContext.SendAsync(command);

        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task ShouldNotCreateUserGivenInValidPassword()
    {
        var command = _testContext.GetCreateUserCommandBuilder()
            .WithUsername("Drake")
            .WithEmail("drake@authenticator.com")
            .WithPhoneNumber("123-456-789")
            .WithPassword("1234")
            .Build();

        var result = await _testContext.SendAsync(command);

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task ShouldNotCreateUserGivenInValidUsername()
    {
        var command = _testContext.GetCreateUserCommandBuilder()
            .WithUsername("Drake&")
            .WithEmail("drake@authenticator.com")
            .WithPhoneNumber("123-456-789")
            .WithPassword("K6#?m412kNSe")
            .Build();

        var result = await _testContext.SendAsync(command);

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }
}
