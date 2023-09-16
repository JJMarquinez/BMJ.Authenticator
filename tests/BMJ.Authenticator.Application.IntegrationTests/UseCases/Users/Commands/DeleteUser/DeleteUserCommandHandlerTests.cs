using BMJ.Authenticator.Application.FunctionalTests.TestContext;
using BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser;
using BMJ.Authenticator.Infrastructure.Identity;

namespace BMJ.Authenticator.Application.FunctionalTests.UseCases.Users.Commands.DeleteUser;

[Collection(nameof(AuthenticatorTestConextCollection))]
public class DeleteUserCommandHandlerTests : IAsyncLifetime
{
    private readonly AuthenticatorTestConext _testContext;
    private readonly Func<Task> _resetDatabase;

    public DeleteUserCommandHandlerTests(AuthenticatorTestConext testContext)
    {
        _testContext = testContext;
        _resetDatabase = _testContext.ResetState;
    }

    public Task DisposeAsync() => _resetDatabase();

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ShouldDeleteUser()
    {
        var applicationUser = ApplicationUser.Builder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        string? userId = await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);

        var command = new DeleteUserCommand { Id = userId };

        var result = await _testContext.SendAsync(command);
        var user = await _testContext.FindAsync(userId!);

        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Null(user);
    }

    [Fact]
    public async Task ShouldThrowArgumentNullExceptionGivenNonExistingUser()
    {
        var command = new DeleteUserCommand { Id = Guid.NewGuid().ToString() };
        await Assert.ThrowsAsync<ArgumentNullException>(() => _testContext.SendAsync(command));
    }
}
