using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Application.FunctionalTests.TestContext;
using BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser;

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
        var userDto = new UserDto
        {
            UserName = "Joe",
            Email = "joe@authenticator.com",
            PhoneNumber = "111-444-777",
            Roles = new[] { "Guest" }
        };
        string? userId = await _testContext.AddAsync(userDto, "M6#?m412kNSH");
        var command = new DeleteUserCommand { Id = userId };

        var result = await _testContext.SendAsync(command);

        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task ShouldThrowArgumentNullExceptionGivenNonExistingUser()
    {
        var command = new DeleteUserCommand { Id = Guid.NewGuid().ToString() };
        await Assert.ThrowsAsync<ArgumentNullException>(() => _testContext.SendAsync(command));
    }
}
