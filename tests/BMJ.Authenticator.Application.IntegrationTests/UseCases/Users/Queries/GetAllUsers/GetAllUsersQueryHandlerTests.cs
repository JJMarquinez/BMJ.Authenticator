using BMJ.Authenticator.Adapter.Common;
using BMJ.Authenticator.Application.FunctionalTests.TestContext;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers;
using BMJ.Authenticator.Infrastructure.Identity;

namespace BMJ.Authenticator.Application.FunctionalTests.UseCases.Users.Queries.GetAllUsers;

[Collection(nameof(AuthenticatorTestConextCollection))]
public class GetAllUsersQueryHandlerTests : IAsyncLifetime
{
    private readonly AuthenticatorTestConext _testContext;
    private readonly Func<Task> _resetDatabase;

    public GetAllUsersQueryHandlerTests(AuthenticatorTestConext testConext)
    {
        _testContext = testConext;
        _resetDatabase = _testContext.ResetState;
    }

    public Task DisposeAsync() => _resetDatabase();

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ShouldReturnAllUsers()
    {
        var applicationUser = ApplicationUser.Builder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);

        var query = new GetAllUsersQuery();
        var result = await _testContext.SendAsync(query);
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.True(result.Value!.Count == 1);
        Assert.Collection(result.Value!,
            userDto =>
            {
                Assert.Equal("Joe", userDto.UserName);
                Assert.Equal("joe@authenticator.com", userDto.Email);
                Assert.Equal("111-444-777", userDto.PhoneNumber);
                Assert.Equal("Guest", userDto.Roles[0]);
            });
    }

    [Fact]
    public async Task ShouldNotReturnAnyUsers()
    {
        var query = new GetAllUsersQuery();
        var result = await _testContext.SendAsync(query);

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }
}
