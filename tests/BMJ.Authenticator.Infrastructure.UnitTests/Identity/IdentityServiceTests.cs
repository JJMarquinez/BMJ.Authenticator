using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Interfaces;
using BMJ.Authenticator.Domain.Common.Results;
using BMJ.Authenticator.Domain.Entities.Users;
using BMJ.Authenticator.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BMJ.Authenticator.Infrastructure.UnitTests.Identity;

public class IdentityServiceTests
{
    Mock<IAuthLogger> _authLogger;
    Mock<UserManager<ApplicationUser>> _userManager;
    private List<ApplicationUser> _users;
    public IdentityServiceTests()
    {
        _users = new List<ApplicationUser>()
         {
              new ApplicationUser() 
              {
                  UserName = "Ven",
                  Email = "ven@authenticator.com",
                  PhoneNumber = "111-222-3333",
                  PasswordHash = "#553zP1k"
              }
        };
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
    public async void ShouldGetAllUsers()
    {
        _userManager.Setup(u => u.Users).Returns(_users.AsQueryable());
        IIdentityService _identityService = new IdentityService(_userManager.Object, _authLogger.Object);

        Result<List<User>?> result = await _identityService.GetAllUserAsync();

        Assert.True(result.IsSuccess());
        Assert.Single(result.GetValue()!);
        Assert.Collection(result.GetValue()!,
            user => 
            {
                Assert.NotNull(user.GetId());
                Assert.Equal("Ven", user.GetUserName());
                Assert.Equal("ven@authenticator.com", user.GetEmail());
                Assert.Equal("111-222-3333", user.GetPhoneNumber()!);
                Assert.Equal("#553zP1k", user.GetPasswordHash());
                Assert.Null(user.GetRoles());
            });
    }
}
