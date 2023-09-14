using BMJ.Authenticator.Adapter.Common;
using BMJ.Authenticator.Infrastructure.Identity;
using BMJ.Authenticator.Infrastructure.IntegrationTests.TextContext;
using System.Text.Json;

namespace BMJ.Authenticator.Infrastructure.IntegrationTests.Identity;

[Collection(nameof(AuthenticatorTestConextCollection))]
public class IdentityServiceTests : IAsyncLifetime
{
    private readonly AuthenticatorTestConext _testContext;
    private readonly Func<Task> _resetDatabase;

    public IdentityServiceTests(AuthenticatorTestConext testContext)
    {
        _testContext = testContext;
        _resetDatabase = _testContext.ResetState;
    }

    public Task DisposeAsync() => _resetDatabase();

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ShouldNotGetAnyUsersGivenNoUserSaved()
    {
        var identityService = _testContext.GetIdentityService();

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
        var identityService = _testContext.GetIdentityService();

        var result = await identityService.GetAllUserAsync();
        var resultValue = JsonSerializer.Deserialize<List<UserIdentification>?>(result.Value!);

        Assert.True(result.Success);
        Assert.Single(resultValue!);
        Assert.Collection(resultValue!,
           user =>
           {
               Assert.Equal(userId, user.Id);
               Assert.Equal(applicationUser.UserName, user.UserName);
               Assert.Equal(applicationUser.Email, user.Email);
               Assert.Equal(applicationUser.PhoneNumber, user.PhoneNumber!);
               Assert.Equal(roles, user.Roles!);
           });
    }

    [Fact]
    public async Task ShouldGetUserById()
    {
        var applicationUser = ApplicationUser.Builder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        string? userId = await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);
        var identityService = _testContext.GetIdentityService();

        var result = await identityService.GetUserByIdAsync(userId!);
        var resultValue = JsonSerializer.Deserialize<UserIdentification>(result.Value!);

        Assert.True(result.Success);
        Assert.Equal(userId, resultValue.Id);
        Assert.Equal(applicationUser.UserName, resultValue.UserName);
        Assert.Equal(applicationUser.Email, resultValue.Email);
        Assert.Equal(applicationUser.PhoneNumber, resultValue.PhoneNumber!);
        Assert.Equal(roles, resultValue.Roles!);
    }

    [Fact]
    public async Task ShouldThrowArgumentNullExceptionWhenGetUserByIdAsyncIsCalledGivenNonexistentUser()
    {
        var identityService = _testContext.GetIdentityService();
        await Assert.ThrowsAsync<ArgumentNullException>(() => identityService.GetUserByIdAsync(Guid.NewGuid().ToString()));
    }

    [Fact]
    public async Task ShouldCreateUser()
    {
        var identityService = _testContext.GetIdentityService();

        var result = await identityService.CreateUserAsync("Jhon", "Jhon1234!", "jhon@auth.com", "67543218"!);

        Assert.True(result.Success);
    }
}
