using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Interfaces;
using BMJ.Authenticator.Domain.Common.Results;
using BMJ.Authenticator.Domain.Entities.Users;
using BMJ.Authenticator.Infrastructure.Common;
using BMJ.Authenticator.Infrastructure.Identity;
using BMJ.Authenticator.Infrastructure.UnitTests.Identity.Builders;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;

namespace BMJ.Authenticator.Infrastructure.UnitTests.Identity;

public class IdentityServiceTests
{
    Mock<IAuthLogger> _authLogger;
    Mock<UserManager<ApplicationUser>> _userManager;
    List<ApplicationUser> _users;
    List<ApplicationUser> _noUsers;
    List<string> _roles;
    string _userId;
    public IdentityServiceTests()
    {
        _userId = "98ac978e-da91-4932-a4b4-7c703e98efc3";
        _noUsers = new List<ApplicationUser>();
        _users = new List<ApplicationUser>()
         {
              ApplicationUserBuilder.New()
            .WithId(_userId)
            .WithUserName("Ven")
            .WithEmail("ven@authenticator.com")
            .WithPhoneNumber("111-222-3333")
            .WithPasswordHash("#553zP1k")
            .Build()
        };
        _roles = new List<string>() { "Administrator", "Standard" };
        _authLogger = new();
        _userManager = MockUserManager(_users);
    }

    public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();
        var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        mgr.Object.UserValidators.Add(new UserValidator<TUser>());
        mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

        mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
        mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
        mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);

        return mgr;
    }


    [Fact]
    public async void ShouldGetAllUse()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        Result<List<User>?> result = await _identityService.GetAllUserAsync();

        Assert.True(result.IsSuccess());
        Assert.Single(result.GetValue()!);
        Assert.Collection(result.GetValue()!,
            user => 
            {
                Assert.Equal(_userId, user.GetId());
                Assert.Equal("Ven", user.GetUserName());
                Assert.Equal("ven@authenticator.com", user.GetEmail());
                Assert.Equal("111-222-3333", user.GetPhoneNumber()!);
                Assert.Equal("#553zP1k", user.GetPasswordHash());
                Assert.Null(user.GetRoles());
            });
    }

    [Fact]
    public async void ShouldGetAllUsersWithRoles()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable());
        _userManager.Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(_roles);
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        Result<List<User>?> result = await _identityService.GetAllUserAsync();

        Assert.True(result.IsSuccess());
        Assert.Single(result.GetValue()!);
        Assert.Collection(result.GetValue()!,
            user =>
            {
                Assert.Equal(_userId, user.GetId());
                Assert.Equal("Ven", user.GetUserName());
                Assert.Equal("ven@authenticator.com", user.GetEmail());
                Assert.Equal("111-222-3333", user.GetPhoneNumber()!);
                Assert.Equal("#553zP1k", user.GetPasswordHash());
                Assert.Equal(_roles, user.GetRoles()!);
            });
    }

    [Fact]
    public async void ShouldNotGetAnyUser()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(_noUsers.AsQueryable());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        Result<List<User>?> result = await _identityService.GetAllUserAsync();

        Assert.True(result.IsFailure());
        Assert.Null(result.GetValue());
        Assert.Equal(InfrastructureError.Identity.ItDoesNotExistAnyUser, result.GetError());
    }

    [Fact]
    public async Task ShouldThrowArgumentNullExceptionGivenAUserWithoutUserName()
    {
        List<ApplicationUser> _notValidUsers = new List<ApplicationUser>() 
        {
            ApplicationUserBuilder.New()
            .WithEmail("ven@authenticator.com")
            .WithPasswordHash("#553zP1k")
            .Build()
        };
        _userManager.Setup(userManager => userManager.Users).Returns(_notValidUsers.AsQueryable());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        await Assert.ThrowsAsync<ArgumentNullException>(_identityService.GetAllUserAsync);
    }

    [Fact]
    public async Task ShouldThrowArgumentExceptionGivenAUserWithEmptyUserName()
    {
        List<ApplicationUser> _notValidUsers = new List<ApplicationUser>()
        {
            ApplicationUserBuilder.New()
            .WithUserName(string.Empty)
            .WithEmail("ven@authenticator.com")
            .WithPasswordHash("#553zP1k")
            .Build()
        };
        _userManager.Setup(userManager => userManager.Users).Returns(_notValidUsers.AsQueryable());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        await Assert.ThrowsAsync<ArgumentException>(_identityService.GetAllUserAsync);
    }

    [Fact]
    public async Task ShouldThrowArgumentNullExceptionGivenAUserWithoutEmail()
    {
        List<ApplicationUser> _notValidUsers = new List<ApplicationUser>()
        {
            ApplicationUserBuilder.New()
            .WithUserName("Ven")
            .WithPasswordHash("#553zP1k")
            .Build()
        };
        _userManager.Setup(userManager => userManager.Users).Returns(_notValidUsers.AsQueryable());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        await Assert.ThrowsAsync<ArgumentNullException>(_identityService.GetAllUserAsync);
    }

    [Fact]
    public async Task ShouldThrowArgumentExceptionGivenAUserWithEmptyEmail()
    {
        List<ApplicationUser> _notValidUsers = new List<ApplicationUser>()
        {
            ApplicationUserBuilder.New()
            .WithUserName("Ven")
            .WithEmail(string.Empty)
            .WithPasswordHash("#553zP1k")
            .Build()
        };
        _userManager.Setup(userManager => userManager.Users).Returns(_notValidUsers.AsQueryable());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        await Assert.ThrowsAsync<ArgumentException>(_identityService.GetAllUserAsync);
    }

    [Fact]
    public async Task ShouldThrowArgumentExceptionGivenAUserWithInvalidEmail()
    {
        List<ApplicationUser> _notValidUsers = new List<ApplicationUser>()
        {
            ApplicationUserBuilder.New()
            .WithUserName("Ven")
            .WithEmail("ven.auth.com")
            .WithPasswordHash("#553zP1k")
            .Build()
        };
        _userManager.Setup(userManager => userManager.Users).Returns(_notValidUsers.AsQueryable());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        await Assert.ThrowsAsync<ArgumentException>(_identityService.GetAllUserAsync);
    }

    [Fact]
    public async Task ShouldThrowArgumentExceptionGivenAUserWithInvalidPhone()
    {
        List<ApplicationUser> _notValidUsers = new List<ApplicationUser>()
        {
            ApplicationUserBuilder.New()
            .WithUserName("Ven")
            .WithEmail("ven@authenticator.com")
            .WithPhoneNumber("673921485")
            .WithPasswordHash("#553zP1k")
            .Build()
        };
        _userManager.Setup(userManager => userManager.Users).Returns(_notValidUsers.AsQueryable());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        await Assert.ThrowsAsync<ArgumentException>(_identityService.GetAllUserAsync);
    }

    [Fact]
    public async void ShouldGetUserById()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        Result<User?> result = await _identityService.GetUserByIdAsync(_userId);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.GetValue());
        Assert.Equal(_userId, result.GetValue()!.GetId());
        Assert.Equal("Ven", result.GetValue()!.GetUserName());
        Assert.Equal("ven@authenticator.com", result.GetValue()!.GetEmail());
        Assert.Equal("111-222-3333", result.GetValue()!.GetPhoneNumber()!);
        Assert.Equal("#553zP1k", result.GetValue()!.GetPasswordHash());
        Assert.Null(result.GetValue()!.GetRoles());
    }

    [Fact]
    public async void ShouldGetUserByIdWithRole()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
        _userManager.Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(_roles);
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        Result<User?> result = await _identityService.GetUserByIdAsync(_userId);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.GetValue());
        Assert.Equal(_userId, result.GetValue()!.GetId());
        Assert.Equal("Ven", result.GetValue()!.GetUserName());
        Assert.Equal("ven@authenticator.com", result.GetValue()!.GetEmail());
        Assert.Equal("111-222-3333", result.GetValue()!.GetPhoneNumber()!);
        Assert.Equal("#553zP1k", result.GetValue()!.GetPasswordHash());
        Assert.Equal(_roles, result.GetValue()!.GetRoles()!);

    }
}
