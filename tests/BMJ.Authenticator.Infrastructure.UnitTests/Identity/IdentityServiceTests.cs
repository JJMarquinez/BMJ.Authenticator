//using BMJ.Authenticator.Adapter.Common.Abstractions;
//using BMJ.Authenticator.Application.Common.Interfaces;
//using BMJ.Authenticator.Domain.Common.Results;
//using BMJ.Authenticator.Domain.Entities.Users;
//using BMJ.Authenticator.Adapter.Common;
//using BMJ.Authenticator.Infrastructure.Identity;
//using Microsoft.AspNetCore.Identity;
//using MockQueryable.Moq;
//using Moq;

//namespace BMJ.Authenticator.Infrastructure.UnitTests.Identity;

//public class IdentityServiceTests
//{
//    Mock<IAuthLogger> _authLogger;
//    Mock<UserManager<ApplicationUser>> _userManager;
//    List<ApplicationUser> _users;
//    List<string> _roles;
//    string _userId;

//    public IdentityServiceTests()
//    {
//        _userId = "98ac978e-da91-4932-a4b4-7c703e98efc3";
//        _users = new List<ApplicationUser>()
//         {
//            ApplicationUser.Builder()
//            .WithId(_userId)
//            .WithUserName("Ven")
//            .WithEmail("ven@authenticator.com")
//            .WithPhoneNumber("111-222-3333")
//            .WithPasswordHash("#553zP1k")
//            .Build()
//        };
//        _roles = new List<string>() { "Administrator", "Standard" };
//        _authLogger = new();
//        _userManager = MockUserManager<ApplicationUser>();
//    }

//    public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
//    {
//        var store = new Mock<IUserStore<TUser>>();
//        var userManager = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
//        userManager.Object.UserValidators.Add(new UserValidator<TUser>());
//        userManager.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
//        return userManager;
//    }


//    [Fact]
//    public async void ShouldGetAllUser()
//    {
//        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable());
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        Result<List<User>?> result = await _identityService.GetAllUserAsync();

//        Assert.True(result.IsSuccess());
//        Assert.Single(result.GetValue()!);
//        Assert.Collection(result.GetValue()!,
//            user => 
//            {
//                Assert.Equal(_userId, user.GetId());
//                Assert.Equal("Ven", user.GetUserName());
//                Assert.Equal("ven@authenticator.com", user.GetEmail());
//                Assert.Equal("111-222-3333", user.GetPhoneNumber()!);
//                Assert.Equal("#553zP1k", user.GetPasswordHash());
//                Assert.Null(user.GetRoles());
//            });
//    }

//    [Fact]
//    public async void ShouldGetAllUsersWithRoles()
//    {
//        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable());
//        _userManager.Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(_roles);
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        Result<List<User>?> result = await _identityService.GetAllUserAsync();

//        Assert.True(result.IsSuccess());
//        Assert.Single(result.GetValue()!);
//        Assert.Collection(result.GetValue()!,
//            user =>
//            {
//                Assert.Equal(_userId, user.GetId());
//                Assert.Equal("Ven", user.GetUserName());
//                Assert.Equal("ven@authenticator.com", user.GetEmail());
//                Assert.Equal("111-222-3333", user.GetPhoneNumber()!);
//                Assert.Equal("#553zP1k", user.GetPasswordHash());
//                Assert.Equal(_roles, user.GetRoles()!);
//            });
//    }

//    [Fact]
//    public async void ShouldNotGetAnyUser()
//    {
//        _userManager.Setup(userManager => userManager.Users).Returns(new List<ApplicationUser>().AsQueryable());
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        Result<List<User>?> result = await _identityService.GetAllUserAsync();

//        Assert.True(result.IsFailure());
//        Assert.Null(result.GetValue());
//        Assert.Equal(InfrastructureError.Identity.ItDoesNotExistAnyUser, result.GetError());
//        _authLogger.Verify(m => m.Warning(It.IsAny<string>()), Times.Once);
//    }

//    [Fact]
//    public async Task ShouldThrowArgumentNullExceptionGivenAUserWithoutUserName()
//    {
//        List<ApplicationUser> _notValidUsers = new List<ApplicationUser>() 
//        {
//            ApplicationUser.Builder()
//            .WithEmail("ven@authenticator.com")
//            .WithPasswordHash("#553zP1k")
//            .Build()
//        };
//        _userManager.Setup(userManager => userManager.Users).Returns(_notValidUsers.AsQueryable());
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        await Assert.ThrowsAsync<ArgumentNullException>(_identityService.GetAllUserAsync);
//    }

//    [Fact]
//    public async Task ShouldThrowArgumentExceptionGivenAUserWithEmptyUserName()
//    {
//        List<ApplicationUser> _notValidUsers = new List<ApplicationUser>()
//        {
//            ApplicationUser.Builder()
//            .WithUserName(string.Empty)
//            .WithEmail("ven@authenticator.com")
//            .WithPasswordHash("#553zP1k")
//            .Build()
//        };
//        _userManager.Setup(userManager => userManager.Users).Returns(_notValidUsers.AsQueryable());
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        await Assert.ThrowsAsync<ArgumentException>(_identityService.GetAllUserAsync);
//    }

//    [Fact]
//    public async Task ShouldThrowArgumentNullExceptionGivenAUserWithoutEmail()
//    {
//        List<ApplicationUser> _notValidUsers = new List<ApplicationUser>()
//        {
//            ApplicationUser.Builder()
//            .WithUserName("Ven")
//            .WithPasswordHash("#553zP1k")
//            .Build()
//        };
//        _userManager.Setup(userManager => userManager.Users).Returns(_notValidUsers.AsQueryable());
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        await Assert.ThrowsAsync<ArgumentNullException>(_identityService.GetAllUserAsync);
//    }

//    [Fact]
//    public async Task ShouldThrowArgumentExceptionGivenAUserWithEmptyEmail()
//    {
//        List<ApplicationUser> _notValidUsers = new List<ApplicationUser>()
//        {
//            ApplicationUser.Builder()
//            .WithUserName("Ven")
//            .WithEmail(string.Empty)
//            .WithPasswordHash("#553zP1k")
//            .Build()
//        };
//        _userManager.Setup(userManager => userManager.Users).Returns(_notValidUsers.AsQueryable());
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        await Assert.ThrowsAsync<ArgumentException>(_identityService.GetAllUserAsync);
//    }

//    [Fact]
//    public async Task ShouldThrowArgumentExceptionGivenAUserWithInvalidEmail()
//    {
//        List<ApplicationUser> _notValidUsers = new List<ApplicationUser>()
//        {
//            ApplicationUser.Builder()
//            .WithUserName("Ven")
//            .WithEmail("ven.auth.com")
//            .WithPasswordHash("#553zP1k")
//            .Build()
//        };
//        _userManager.Setup(userManager => userManager.Users).Returns(_notValidUsers.AsQueryable());
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        await Assert.ThrowsAsync<ArgumentException>(_identityService.GetAllUserAsync);
//    }

//    [Fact]
//    public async Task ShouldThrowArgumentExceptionGivenAUserWithInvalidPhone()
//    {
//        List<ApplicationUser> _notValidUsers = new List<ApplicationUser>()
//        {
//            ApplicationUser.Builder()
//            .WithUserName("Ven")
//            .WithEmail("ven@authenticator.com")
//            .WithPhoneNumber("673921485")
//            .WithPasswordHash("#553zP1k")
//            .Build()
//        };
//        _userManager.Setup(userManager => userManager.Users).Returns(_notValidUsers.AsQueryable());
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        await Assert.ThrowsAsync<ArgumentException>(_identityService.GetAllUserAsync);
//    }

//    [Fact]
//    public async void ShouldGetUserById()
//    {
//        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        Result<User?> result = await _identityService.GetUserByIdAsync(_userId);

//        Assert.True(result.IsSuccess());
//        Assert.NotNull(result.GetValue());
//        Assert.Equal(_userId, result.GetValue()!.GetId());
//        Assert.Equal("Ven", result.GetValue()!.GetUserName());
//        Assert.Equal("ven@authenticator.com", result.GetValue()!.GetEmail());
//        Assert.Equal("111-222-3333", result.GetValue()!.GetPhoneNumber()!);
//        Assert.Equal("#553zP1k", result.GetValue()!.GetPasswordHash());
//        Assert.Null(result.GetValue()!.GetRoles());
//    }

//    [Fact]
//    public async void ShouldGetUserByIdWithRole()
//    {
//        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
//        _userManager.Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(_roles);
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        Result<User?> result = await _identityService.GetUserByIdAsync(_userId);

//        Assert.True(result.IsSuccess());
//        Assert.NotNull(result.GetValue());
//        Assert.Equal(_userId, result.GetValue()!.GetId());
//        Assert.Equal("Ven", result.GetValue()!.GetUserName());
//        Assert.Equal("ven@authenticator.com", result.GetValue()!.GetEmail());
//        Assert.Equal("111-222-3333", result.GetValue()!.GetPhoneNumber()!);
//        Assert.Equal("#553zP1k", result.GetValue()!.GetPasswordHash());
//        Assert.Equal(_roles, result.GetValue()!.GetRoles()!);
//    }

//    [Fact]
//    public void ShouldThrowNullReferenceExceptionWhenGetUserByIdAsyncIsCalledGivenNonexistentUser()
//    {
//        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
//        _userManager.Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(_roles);
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        _ = Assert.ThrowsAsync<NullReferenceException>(async() => await _identityService.GetUserByIdAsync(Guid.NewGuid().ToString()));
//    }

//    [Fact]
//    public async void ShouldCreateUser()
//    {
//        _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        Result<string?> result = await _identityService.CreateUserAsync("Jhon", "Jhon1234!", "jhon@auth.com", "67543218");

//        Assert.True(result.IsSuccess());
//        Assert.NotNull(result.GetValue());
//    }

//    [Fact]
//    public async void ShouldNotCreateUser()
//    {
//        _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        Result<string?> result = await _identityService.CreateUserAsync("Jhon", "1234", "jhonauth.com", "67543218");

//        Assert.True(result.IsFailure());
//        Assert.Equal(result.GetError(), InfrastructureError.Identity.UserWasNotCreated);
//        _authLogger.Verify(m => m.Error(It.IsAny<string>(), It.IsAny<IEnumerable<IdentityError>>(), It.IsAny<ApplicationUser>()), Times.Once);
//    }

//    [Fact]
//    public async void ShouldUpdateUser()
//    {
//        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
//        _userManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        Result result = await _identityService.UpdateUserAsync(_userId, "Jhon", "jhon@auth.com", "67543218");

//        Assert.True(result.IsSuccess());
//    }

//    [Fact]
//    public async void ShouldNotUpdateUser()
//    {
//        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
//        _userManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Failed());
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        Result result = await _identityService.UpdateUserAsync(_userId, "Jhon", "jhon@auth.com", "67543218");

//        Assert.True(result.IsFailure());
//        _authLogger.Verify(m => m.Error(It.IsAny<string>(), It.IsAny<IEnumerable<IdentityError>>(), It.IsAny<ApplicationUser>()), Times.Once);
//    }

//    [Fact]
//    public void ShouldThrowNullReferenceExceptionWhenUpdateUserAsyncIsCalledGivenNonexistentUser()
//    {
//        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        _ = Assert.ThrowsAsync<NullReferenceException>(async () => await _identityService.UpdateUserAsync(Guid.NewGuid().ToString(), "Jhon", "jhon@auth.com", "67543218"));
//    }

//    [Fact]
//    public async void ShouldDeleteUser()
//    {
//        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
//        _userManager.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        Result result = await _identityService.DeleteUserAsync(_userId);

//        Assert.True(result.IsSuccess());
//    }

//    [Fact]
//    public async void ShouldNotDeleteUser()
//    {
//        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
//        _userManager.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Failed());
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        Result result = await _identityService.DeleteUserAsync(_userId);

//        Assert.True(result.IsFailure());
//        _authLogger.Verify(m => m.Error(It.IsAny<string>(), It.IsAny<IEnumerable<IdentityError>>(), It.IsAny<ApplicationUser>()), Times.Once);
//    }

//    [Fact]
//    public async void ShouldAuthenticateUser()
//    {
//        _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(_users.FirstOrDefault());
//        _userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);
//        _userManager.Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(_roles);

//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        Result<User?> result = await _identityService.AuthenticateMemberAsync("Ven", "#553zP1k");

//        Assert.True(result.IsSuccess());
//        Assert.Equal(_userId, result.GetValue()!.GetId());
//        Assert.Equal("Ven", result.GetValue()!.GetUserName());
//        Assert.Equal("ven@authenticator.com", result.GetValue()!.GetEmail());
//        Assert.Equal("111-222-3333", result.GetValue()!.GetPhoneNumber()!);
//        Assert.Equal("#553zP1k", result.GetValue()!.GetPasswordHash());
//        Assert.Equal(_roles, result.GetValue()!.GetRoles()!);
//    }

//    [Fact]
//    public async void ShouldAuthenticateUserWithourRoles()
//    {
//        _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(_users.FirstOrDefault());
//        _userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);

//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        Result<User?> result = await _identityService.AuthenticateMemberAsync("Ven", "#553zP1k");

//        Assert.True(result.IsSuccess());
//        Assert.Equal(_userId, result.GetValue()!.GetId());
//        Assert.Equal("Ven", result.GetValue()!.GetUserName());
//        Assert.Equal("ven@authenticator.com", result.GetValue()!.GetEmail());
//        Assert.Equal("111-222-3333", result.GetValue()!.GetPhoneNumber()!);
//        Assert.Equal("#553zP1k", result.GetValue()!.GetPasswordHash());
//        Assert.Null(result.GetValue()!.GetRoles());
//    }

//    [Fact]
//    public async void ShouldNotAuthenticateUserGivenWrongUsername()
//    {
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        Result<User?> result = await _identityService.AuthenticateMemberAsync("Ven", "#553zP1k");

//        Assert.True(result.IsFailure());
//        _authLogger.Verify(m => m.Warning(It.IsAny<string>()), Times.Once);
//    }

//    [Fact]
//    public async void ShouldNotAuthenticateUserGivenWrongPassword()
//    {
//        _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(_users.FirstOrDefault());
//        _userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(false);

//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        Result<User?> result = await _identityService.AuthenticateMemberAsync("Ven", "#553zP1k");

//        Assert.True(result.IsFailure());
//        _authLogger.Verify(m => m.Warning(It.IsAny<string>()), Times.Once);
//    }

//    [Fact]
//    public void ShouldNotFindUserByUsername()
//    {
//        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        bool result = _identityService.DoesUserNameNotExist("Ven");

//        Assert.False(result);
//    }

//    [Fact]
//    public void ShouldFindUserByUsername()
//    {
//        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        bool result = _identityService.DoesUserNameNotExist("Maberic");

//        Assert.True(result);
//    }

//    [Fact]
//    public void ShouldFindUserId()
//    {
//        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        bool result = _identityService.IsUserIdAssigned(_userId);

//        Assert.True(result);
//    }

//    [Fact]
//    public void ShouldNotFindUserId()
//    {
//        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
//        IIdentityAdapter _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

//        bool result = _identityService.IsUserIdAssigned(Guid.NewGuid().ToString());

//        Assert.False(result);
//    }
//}
