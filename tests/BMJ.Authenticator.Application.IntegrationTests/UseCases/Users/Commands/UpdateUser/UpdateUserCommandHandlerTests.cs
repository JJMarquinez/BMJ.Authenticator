using BMJ.Authenticator.Adapter.Common;
using BMJ.Authenticator.Application.FunctionalTests.TestContext;
using BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser;
using BMJ.Authenticator.Infrastructure.Identity;

namespace BMJ.Authenticator.Application.FunctionalTests.UseCases.Users.Commands.UpdateUser;

[Collection(nameof(AuthenticatorTestConextCollection))]
public class UpdateUserCommandHandlerTests : IAsyncLifetime
{
    private readonly AuthenticatorTestConext _testContext;
    private readonly Func<Task> _resetDatabase;

    public UpdateUserCommandHandlerTests(AuthenticatorTestConext testContext)
    {
        _testContext = testContext;
        _resetDatabase = _testContext.ResetState;
    }

    public Task DisposeAsync() => _resetDatabase();

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ShouldUpdateUser()
    {
        var applicationUser = _testContext.GetApplicationUserBuilder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        string? userId = await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);

        var command = new UpdateUserCommand
        {
            Id = userId,
            UserName = "Drake",
            Email = "drake@authenticator.com",
            PhoneNumber = "123-456-789",
        };

        var result = await _testContext.SendAsync(command);
        var user = await _testContext.FindAsync(userId!);

        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(user);
        Assert.Equal(command.UserName, user.UserName);
        Assert.Equal(command.Email, user.Email);
        Assert.Equal(command.PhoneNumber, user.PhoneNumber);
    }

    [Fact]
    public async Task ShouldThrowNullReferenceExceptionGivenNonExistingUser()
    {
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Drake",
            Email = "drake@authenticator.com",
            PhoneNumber = "123-456-789",
        };

        await Assert.ThrowsAsync<NullReferenceException>(() => _testContext.SendAsync(command));
    }
}
