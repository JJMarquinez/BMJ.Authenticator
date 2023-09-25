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
        var identityAdapter = _testContext.GetIdentityAdapter();

        var result = await identityAdapter.GetAllUserAsync();

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task ShouldGetAllUsers()
    {
        var applicationUser = _testContext.GetApplicationUserBuilder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        string? userId = await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);
        var identityAdapter = _testContext.GetIdentityAdapter();

        var result = await identityAdapter.GetAllUserAsync();

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

    [Fact]
    public async Task ShouldGetUserById()
    {
        var applicationUser = _testContext.GetApplicationUserBuilder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        string? userId = await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);
        var identityAdapter = _testContext.GetIdentityAdapter();

        var result = await identityAdapter.GetUserByIdAsync(userId!);

        Assert.True(result.Success);
        Assert.Equal(userId, result.Value!.Id);
        Assert.Equal(applicationUser.UserName, result.Value.UserName);
        Assert.Equal(applicationUser.Email, result.Value.Email);
        Assert.Equal(applicationUser.PhoneNumber, result.Value.PhoneNumber!);
        Assert.Equal(roles, result.Value.Roles!);
    }

    [Fact]
    public async Task ShouldThrowArgumentNullExceptionWhenGetUserByIdAsyncIsCalledGivenNonexistentUser()
    {
        var identityAdapter = _testContext.GetIdentityAdapter();
        await Assert.ThrowsAsync<ArgumentNullException>(() => identityAdapter.GetUserByIdAsync(Guid.NewGuid().ToString()));
    }

    [Fact]
    public async Task ShouldCreateUser()
    {
        var identityAdapter = _testContext.GetIdentityAdapter();

        var result = await identityAdapter.CreateUserAsync("Jhon", "Jhon1234!", "jhon@auth.com", "67543218"!);

        Assert.True(result.Success);
    }

    [Fact]
    public async Task ShouldThrowArgumentNullExceptionGivenNullPassword()
    {
        var identityAdapter = _testContext.GetIdentityAdapter();

        await Assert.ThrowsAsync<ArgumentNullException>(() => identityAdapter.CreateUserAsync("Jhon", null!, "jhon@auth.com", "67543218"!));
    }

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
        var identityAdapter = _testContext.GetIdentityAdapter();

        var result = await identityAdapter.UpdateUserAsync(userId!, "Jhon", "jhon@auth.com", "67543218");
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
        var applicationUser = _testContext.GetApplicationUserBuilder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        string? userId = await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);
        var identityAdapter = _testContext.GetIdentityAdapter();

        var result = await identityAdapter.UpdateUserAsync(userId!, null!, "jhon@auth.com", "67543218");

        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task ShouldThrowNullReferenceExceptionGivenNonExistingUser()
    {
        var identityAdapter = _testContext.GetIdentityAdapter();

        await Assert.ThrowsAsync<NullReferenceException>(() => identityAdapter.UpdateUserAsync(Guid.NewGuid().ToString(), "Jhon", "jhon@auth.com", "67543218"));
    }

    [Fact]
    public async Task ShouldDeleteUser()
    {
        var applicationUser = _testContext.GetApplicationUserBuilder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        string? userId = await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);
        var identityAdapter = _testContext.GetIdentityAdapter();

        var result = await identityAdapter.DeleteUserAsync(userId!);
        var user = await _testContext.FindAsync(userId!);

        Assert.True(result.Success);
        Assert.Null(user);
    }

    [Fact]
    public async Task ShouldArgumentNullExceptionGivenInvalidUserId()
    {
        var identityAdapter = _testContext.GetIdentityAdapter();

        await Assert.ThrowsAsync<ArgumentNullException>(() => identityAdapter.DeleteUserAsync(Guid.NewGuid().ToString()));
    }

    [Fact]
    public async Task ShouldAuthenticateUser()
    {
        var applicationUser = _testContext.GetApplicationUserBuilder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);
        var identityAdapter = _testContext.GetIdentityAdapter();

        var result = await identityAdapter.AuthenticateMemberAsync("Joe", "M6#?m412kNSH");

        Assert.True(result.Success);
        Assert.Equal(applicationUser.UserName, result.Value!.UserName);
        Assert.Equal(applicationUser.Email, result.Value.Email);
        Assert.Equal(applicationUser.PhoneNumber, result.Value.PhoneNumber!);
        Assert.Equal(roles, result.Value.Roles!);
    }

    [Fact]
    public async Task ShouldNotAuthenticateUserGivenWronCredentials()
    {
        var applicationUser = _testContext.GetApplicationUserBuilder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);

        var identityAdapter = _testContext.GetIdentityAdapter();

        var result = await identityAdapter.AuthenticateMemberAsync("Jou", "M6#?m412kNSH-p-");

        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task ShouldFindUserByUsername()
    {
        var applicationUser = _testContext.GetApplicationUserBuilder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);

        var identityAdapter = _testContext.GetIdentityAdapter();

        var result = identityAdapter.DoesUserNameNotExist("Joe");

        Assert.False(result);
    }

    [Fact]
    public void ShouldNotFindUserByUsernameGivenNonExistingUsername()
    {
        var identityAdapter = _testContext.GetIdentityAdapter();

        var result = identityAdapter.DoesUserNameNotExist("Alex");

        Assert.True(result);
    }

    [Fact]
    public async Task ShouldFindUserId()
    {
        var applicationUser = _testContext.GetApplicationUserBuilder()
            .WithUserName("Joe")
            .WithEmail("joe@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        var userId = await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);

        var identityAdapter = _testContext.GetIdentityAdapter();

        var result = identityAdapter.IsUserIdAssigned(userId!);

        Assert.True(result);
    }

    [Fact]
    public void ShouldNotFindUserIdGivenNonExistingUserId()
    {
        var identityAdapter = _testContext.GetIdentityAdapter();

        var result = identityAdapter.IsUserIdAssigned(Guid.NewGuid().ToString());

        Assert.False(result);
    }
}
