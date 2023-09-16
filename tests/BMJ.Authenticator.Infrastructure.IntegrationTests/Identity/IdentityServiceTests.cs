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

    [Fact]
    public async Task ShouldThrowArgumentNullExceptionGivenNullPassword()
    {
        var identityService = _testContext.GetIdentityService();

        await Assert.ThrowsAsync<ArgumentNullException>(() => identityService.CreateUserAsync("Jhon", null!, "jhon@auth.com", "67543218"!));
    }

    [Fact]
    public async Task ShouldUpdateUser()
    {
        var applicationUser = ApplicationUser.Builder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        string? userId = await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);
        var identityService = _testContext.GetIdentityService();

        var result = await identityService.UpdateUserAsync(userId!, "Jhon", "jhon@auth.com", "67543218");
        var user = await _testContext.FindAsync(userId!);

        Assert.True(result.Success);
        Assert.NotNull(user);
        Assert.Equal("Jhon", user.UserName);
        Assert.Equal("jhon@auth.com", user.Email);
        Assert.Equal("67543218", user.PhoneNumber);
    }

    [Fact]
    public async Task ShouldNotUpdateUserGivenNullUsername()
    {
        var applicationUser = ApplicationUser.Builder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        string? userId = await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);
        var identityService = _testContext.GetIdentityService();

        var result = await identityService.UpdateUserAsync(userId!, null!, "jhon@auth.com", "67543218");

        Assert.False(result.Success);
        Assert.Equal(InfrastructureError.Identity.UserWasNotUpdated, result.Error);
    }

    [Fact]
    public async Task ShouldThrowNullReferenceExceptionGivenNonExistingUser()
    {
        var identityService = _testContext.GetIdentityService();

        await Assert.ThrowsAsync<NullReferenceException>(() => identityService.UpdateUserAsync(Guid.NewGuid().ToString(), "Jhon", "jhon@auth.com", "67543218"));
    }

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
        var identityService = _testContext.GetIdentityService();

        var result = await identityService.DeleteUserAsync(userId!);
        var user = await _testContext.FindAsync(userId!);

        Assert.True(result.Success);
        Assert.Null(user);
    }

    [Fact]
    public async Task ShouldArgumentNullExceptionGivenInvalidUserId()
    {
        var identityService = _testContext.GetIdentityService();

        await Assert.ThrowsAsync<ArgumentNullException>(() => identityService.DeleteUserAsync(Guid.NewGuid().ToString()));
    }

    [Fact]
    public async Task ShouldAuthenticateUser()
    {
        var applicationUser = ApplicationUser.Builder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);
        var identityService = _testContext.GetIdentityService();

        var result = await identityService.AuthenticateMemberAsync("Joe", "M6#?m412kNSH");
        var resultValue = JsonSerializer.Deserialize<UserIdentification>(result.Value!);

        Assert.True(result.Success);
        Assert.Equal(applicationUser.UserName, resultValue.UserName);
        Assert.Equal(applicationUser.Email, resultValue.Email);
        Assert.Equal(applicationUser.PhoneNumber, resultValue.PhoneNumber!);
        Assert.Equal(roles, resultValue.Roles!);
    }

    [Fact]
    public async Task ShouldNotAuthenticateUserGivenWronCredentials()
    {
        var applicationUser = ApplicationUser.Builder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);

        var identityService = _testContext.GetIdentityService();

        var result = await identityService.AuthenticateMemberAsync("Jou", "M6#?m412kNSH-p-");

        Assert.False(result.Success);
        Assert.Equal(InfrastructureError.Identity.UserNameOrPasswordNotValid, result.Error);
    }

    [Fact]
    public async Task ShouldFindUserByUsername()
    {
        var applicationUser = ApplicationUser.Builder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);

        var identityService = _testContext.GetIdentityService();

        var result = identityService.DoesUserNameNotExist("Joe");

        Assert.False(result);
    }

    [Fact]
    public void ShouldNotFindUserByUsernameGivenNonExistingUsername()
    {
        var identityService = _testContext.GetIdentityService();

        var result = identityService.DoesUserNameNotExist("Alex");

        Assert.True(result);
    }

    [Fact]
    public async Task ShouldFindUserId()
    {
        var applicationUser = ApplicationUser.Builder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        var userId = await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);

        var identityService = _testContext.GetIdentityService();

        var result = identityService.IsUserIdAssigned(userId!);

        Assert.True(result);
    }

    [Fact]
    public void ShouldNotFindUserIdGivenNonExistingUserId()
    {
        var identityService = _testContext.GetIdentityService();

        var result = identityService.IsUserIdAssigned(Guid.NewGuid().ToString());

        Assert.False(result);
    }
}
