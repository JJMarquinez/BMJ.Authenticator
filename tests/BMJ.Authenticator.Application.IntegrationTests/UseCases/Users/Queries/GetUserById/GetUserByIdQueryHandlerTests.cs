using BMJ.Authenticator.Application.FunctionalTests.TestContext;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById;
using BMJ.Authenticator.Infrastructure.Identity;

namespace BMJ.Authenticator.Application.FunctionalTests.UseCases.Users.Queries.GetUserById;

[Collection(nameof(AuthenticatorTestConextCollection))]
public class GetUserByIdQueryHandlerTests : IAsyncLifetime
{
    private readonly AuthenticatorTestConext _testContext;
    private readonly Func<Task> _resetDatabase;

    public GetUserByIdQueryHandlerTests(AuthenticatorTestConext testContext)
    {
        _testContext = testContext;
        _resetDatabase = _testContext.ResetState;
    }

    public Task DisposeAsync() => _resetDatabase();

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ShouldGetAnUser()
    {
        var applicationUser = _testContext.GetApplicationUserBuilder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        string? userId =  await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);

        var query = new GetUserByIdQuery { Id = userId };
        var result = await _testContext.SendAsync(query);

        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(userId, result.Value!.Id);
        Assert.Equal(applicationUser.UserName, result.Value.UserName);
        Assert.Equal(applicationUser.Email, result.Value.Email);
        Assert.Equal(applicationUser.PhoneNumber, result.Value.PhoneNumber);
        Assert.Equal(roles[0], result.Value.Roles[0]);
    }

    [Fact]
    public async Task ShouldThrowArgumentNullExceptionGivenNonExistingUserId()
    {
        var query = new GetUserByIdQuery { Id = Guid.NewGuid().ToString() };
        await Assert.ThrowsAsync<ArgumentNullException>(() => _testContext.SendAsync(query));
    }

    [Fact]
    public async Task ShouldThrowArgumentNullExceptionGivenNullUserId()
    {
        var query = new GetUserByIdQuery();
        await Assert.ThrowsAsync<ArgumentNullException>(() => _testContext.SendAsync(query));
    }
}
