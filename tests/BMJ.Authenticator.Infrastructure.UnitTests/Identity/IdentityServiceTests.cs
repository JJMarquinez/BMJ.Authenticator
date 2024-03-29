﻿using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Adapter.Common;
using BMJ.Authenticator.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;
using System.Text.Json;
using System.Collections.Generic;

namespace BMJ.Authenticator.Infrastructure.UnitTests.Identity;

public class IdentityServiceTests
{
    private readonly Mock<IAuthLogger> _authLogger;
    private readonly Mock<UserManager<ApplicationUser>> _userManager;
    private readonly List<ApplicationUser> _users;
    private readonly List<string> _roles;
    private readonly string _userId;

    public IdentityServiceTests()
    {
        _userId = "98ac978e-da91-4932-a4b4-7c703e98efc3";
        _users = new List<ApplicationUser>()
         {
            ApplicationUser.Builder()
            .WithId(_userId)
            .WithUserName("Ven")
            .WithEmail("ven@authenticator.com")
            .WithPhoneNumber("111-222-3333")
            .WithPasswordHash("#553zP1k")
            .Build()
        };
        _roles = new List<string>() { "Administrator", "Standard" };
        _authLogger = new();
        _userManager = MockUserManager<ApplicationUser>();
    }

    public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();
        var userManager = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        userManager.Object.UserValidators.Add(new UserValidator<TUser>());
        userManager.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
        return userManager;
    }


    [Fact]
    public async void ShouldGetAllUser()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        var result = await _identityService.GetAllUserAsync();
        var resultValue = JsonSerializer.Deserialize<List<UserIdentification>?>(result.Value!);

        Assert.True(result.Success);
        Assert.Single(resultValue!);
        Assert.Collection(resultValue!,
            user =>
            {
                Assert.Equal(_userId, user.Id);
                Assert.Equal("Ven", user.UserName);
                Assert.Equal("ven@authenticator.com", user.Email);
                Assert.Equal("111-222-3333", user.PhoneNumber!);
                Assert.Null(user.Roles);
            });
    }

    [Fact]
    public async void ShouldGetAllUsersWithRoles()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable());
        _userManager.Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(_roles);
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        var result = await _identityService.GetAllUserAsync();
        var resultValue = JsonSerializer.Deserialize<List<UserIdentification>?>(result.Value!);

        Assert.True(result.Success);
        Assert.Single(resultValue!);
        Assert.Collection(resultValue!,
            user =>
            {
                Assert.Equal(_userId, user.Id);
                Assert.Equal("Ven", user.UserName);
                Assert.Equal("ven@authenticator.com", user.Email);
                Assert.Equal("111-222-3333", user.PhoneNumber!);
                Assert.Equal(_roles, user.Roles!);
            });
    }

    [Fact]
    public async void ShouldNotGetAnyUser()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(new List<ApplicationUser>().AsQueryable());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        var result = await _identityService.GetAllUserAsync();

        Assert.True(!result.Success);
        Assert.Null(result.Value);
        Assert.Equal(InfrastructureError.Identity.ItDoesNotExistAnyUser, result.Error);
        _authLogger.Verify(m => m.Warning(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async void ShouldGetUserById()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        var result = await _identityService.GetUserByIdAsync(_userId);
        var resultValue = JsonSerializer.Deserialize<UserIdentification>(result.Value!);

        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal(_userId, resultValue.Id);
        Assert.Equal("Ven", resultValue.UserName);
        Assert.Equal("ven@authenticator.com", resultValue.Email);
        Assert.Equal("111-222-3333", resultValue.PhoneNumber);
        Assert.Null(resultValue.Roles);
    }

    [Fact]
    public async void ShouldGetUserByIdWithRole()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
        _userManager.Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(_roles);
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        var result = await _identityService.GetUserByIdAsync(_userId);
        var resultValue = JsonSerializer.Deserialize<UserIdentification>(result.Value!);

        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal(_userId, resultValue.Id);
        Assert.Equal("Ven", resultValue.UserName);
        Assert.Equal("ven@authenticator.com", resultValue.Email);
        Assert.Equal("111-222-3333", resultValue.PhoneNumber!);
        Assert.Equal(_roles, resultValue.Roles!);
    }

    [Fact]
    public void ShouldThrowNullReferenceExceptionWhenGetUserByIdAsyncIsCalledGivenNonexistentUser()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
        _userManager.Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(_roles);
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        _ = Assert.ThrowsAsync<NullReferenceException>(async () => await _identityService.GetUserByIdAsync(Guid.NewGuid().ToString()));
    }

    [Fact]
    public async void ShouldCreateUser()
    {
        _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        var result = await _identityService.CreateUserAsync("Jhon", "Jhon1234!", "jhon@auth.com", "67543218");

        Assert.True(result.Success);
    }

    [Fact]
    public async void ShouldNotCreateUser()
    {
        _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        var result = await _identityService.CreateUserAsync("Jhon", "1234", "jhonauth.com", "67543218");

        Assert.True(!result.Success);
        Assert.Equal(result.Error, InfrastructureError.Identity.UserWasNotCreated);
        _authLogger.Verify(m => m.Error(It.IsAny<string>(), It.IsAny<IEnumerable<IdentityError>>(), It.IsAny<ApplicationUser>()), Times.Once);
    }

    [Fact]
    public async void ShouldUpdateUser()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
        _userManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        var result = await _identityService.UpdateUserAsync(_userId, "Jhon", "jhon@auth.com", "67543218");

        Assert.True(result.Success);
    }

    [Fact]
    public async void ShouldNotUpdateUser()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
        _userManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Failed());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        var result = await _identityService.UpdateUserAsync(_userId, "Jhon", "jhon@auth.com", "67543218");

        Assert.True(!result.Success);
        _authLogger.Verify(m => m.Error(It.IsAny<string>(), It.IsAny<IEnumerable<IdentityError>>(), It.IsAny<ApplicationUser>()), Times.Once);
    }

    [Fact]
    public void ShouldThrowNullReferenceExceptionWhenUpdateUserAsyncIsCalledGivenNonexistentUser()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        _ = Assert.ThrowsAsync<NullReferenceException>(async () => await _identityService.UpdateUserAsync(Guid.NewGuid().ToString(), "Jhon", "jhon@auth.com", "67543218"));
    }

    [Fact]
    public async void ShouldDeleteUser()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
        _userManager.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        var result = await _identityService.DeleteUserAsync(_userId);

        Assert.True(result.Success);
    }

    [Fact]
    public async void ShouldNotDeleteUser()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
        _userManager.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Failed());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        var result = await _identityService.DeleteUserAsync(_userId);

        Assert.True(!result.Success);
        _authLogger.Verify(m => m.Error(It.IsAny<string>(), It.IsAny<IEnumerable<IdentityError>>(), It.IsAny<ApplicationUser>()), Times.Once);
    }

    [Fact]
    public async void ShouldAuthenticateUser()
    {
        _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(_users.FirstOrDefault());
        _userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);
        _userManager.Setup(userManager => userManager.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(_roles);

        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        var result = await _identityService.AuthenticateMemberAsync("Ven", "#553zP1k");
        var resultValue = JsonSerializer.Deserialize<UserIdentification>(result.Value!);

        Assert.True(result.Success);
        Assert.Equal(_userId, resultValue.Id);
        Assert.Equal("Ven", resultValue.UserName);
        Assert.Equal("ven@authenticator.com", resultValue.Email);
        Assert.Equal("111-222-3333", resultValue.PhoneNumber!);
        Assert.Equal(_roles, resultValue.Roles!);
    }

    [Fact]
    public async void ShouldAuthenticateUserWithourRoles()
    {
        _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(_users.FirstOrDefault());
        _userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);

        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        var result = await _identityService.AuthenticateMemberAsync("Ven", "#553zP1k");
        var resultValue = JsonSerializer.Deserialize<UserIdentification>(result.Value!);

        Assert.True(result.Success);
        Assert.Equal(_userId, resultValue.Id);
        Assert.Equal("Ven", resultValue.UserName);
        Assert.Equal("ven@authenticator.com", resultValue.Email);
        Assert.Equal("111-222-3333", resultValue.PhoneNumber!);
        Assert.Null(resultValue.Roles);
    }

    [Fact]
    public async void ShouldNotAuthenticateUserGivenWrongUsername()
    {
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        var result = await _identityService.AuthenticateMemberAsync("Ven", "#553zP1k");

        Assert.True(!result.Success);
        _authLogger.Verify(m => m.Warning(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async void ShouldNotAuthenticateUserGivenWrongPassword()
    {
        _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(_users.FirstOrDefault());
        _userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(false);

        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        var result = await _identityService.AuthenticateMemberAsync("Ven", "#553zP1k");

        Assert.True(!result.Success);
        _authLogger.Verify(m => m.Warning(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void ShouldNotFindUserByUsername()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        bool result = _identityService.DoesUserNameNotExist("Ven");

        Assert.False(result);
    }

    [Fact]
    public void ShouldFindUserByUsername()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        bool result = _identityService.DoesUserNameNotExist("Maberic");

        Assert.True(result);
    }

    [Fact]
    public void ShouldFindUserId()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        bool result = _identityService.IsUserIdAssigned(_userId);

        Assert.True(result);
    }

    [Fact]
    public void ShouldNotFindUserId()
    {
        _userManager.Setup(userManager => userManager.Users).Returns(_users.AsQueryable().BuildMock());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        bool result = _identityService.IsUserIdAssigned(Guid.NewGuid().ToString());

        Assert.False(result);
    }
}
