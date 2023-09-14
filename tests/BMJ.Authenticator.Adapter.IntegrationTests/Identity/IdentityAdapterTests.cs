using BMJ.Authenticator.Adapter.Common;
using BMJ.Authenticator.Adapter.IntegrationTests.TextContext;
using BMJ.Authenticator.Infrastructure.Identity;

namespace BMJ.Authenticator.Adapter.IntegrationTests.Identity;

[Collection(nameof(AuthenticatorTestConextCollection))]
public class IdentityAdapterTests : IAsyncLifetime
{
    private readonly AuthenticatorTestConext _testContext;
    private readonly Func<Task> _resetDatabase;

    public IdentityAdapterTests(AuthenticatorTestConext testContext)
    {
        _testContext = testContext;
        _resetDatabase = _testContext.ResetState;
    }

    public Task DisposeAsync() => _resetDatabase();

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ShouldNotGetAnyUsersGivenNoUserSaved()
    {
        var identityService = _testContext.GetIdentityAdapter();

        var result = await identityService.GetAllUserAsync();

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(InfrastructureError.Identity.ItDoesNotExistAnyUser, result.Error);
    }

    [Fact]
    public async Task ShouldGetAllUsers()
    {
        var applicationUser = ApplicationUser.Builder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        string? userId = await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);
        var identityService = _testContext.GetIdentityAdapter();

        var result = await identityService.GetAllUserAsync();

        Assert.True(result.Success);
        Assert.Single(result.Value!);
        Assert.Collection(result.Value!,
           user =>
           {
               Assert.Equal(userId, user.Id);
               Assert.Equal(applicationUser.UserName, user.UserName);
               Assert.Equal(applicationUser.Email, user.Email);
               Assert.Equal(applicationUser.PhoneNumber, user.PhoneNumber!);
               Assert.Equal(roles, user.Roles!);
           });
    }
}
