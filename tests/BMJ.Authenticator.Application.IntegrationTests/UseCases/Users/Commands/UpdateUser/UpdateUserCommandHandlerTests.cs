using BMJ.Authenticator.Adapter.Common;
using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Application.FunctionalTests.TestContext;
using BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser;

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
        var userDto = new UserDto
        {
            UserName = "Joe",
            Email = "joe@authenticator.com",
            PhoneNumber = "111-444-777",
            Roles = new[] { "Guest" }
        };
        string? userId = await _testContext.AddAsync(userDto, "M6#?m412kNSH");
        var command = new UpdateUserCommand
        {
            Id = userId,
            UserName = "Drake",
            Email = "drake@authenticator.com",
            PhoneNumber = "123-456-789",
        };

        var result = await _testContext.SendAsync(command);

        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task ShouldThrowNullReferenceExceptionGivenNonExistingUser()
    {
        var error = InfrastructureError.Identity.UserWasNotUpdated;
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
