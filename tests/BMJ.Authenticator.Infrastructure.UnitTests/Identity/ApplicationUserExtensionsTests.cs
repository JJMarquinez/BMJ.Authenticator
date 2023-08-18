using BMJ.Authenticator.Domain.Entities.Users;
using BMJ.Authenticator.Infrastructure.Identity;
using BMJ.Authenticator.Infrastructure.Identity.Builders;

namespace BMJ.Authenticator.Infrastructure.UnitTests.Identity;

public class ApplicationUserExtensionsTests
{
    [Fact]
    public void ShouldConvertApplicationUserToUser() 
    {
        ApplicationUser applicationUser = 
            ApplicationUser.Builder()
            .WithId("8sIi8NZcD34l")
            .WithUserName("Jame")
            .WithEmail("jame@auth.com")
            .WithPhoneNumber("111-222-3333")
            .WithPasswordHash("tOcR1%oH6F0B")
            .Build();
        string[] roles = new[] { "Standard" };

        User user = applicationUser.ToUser(roles);

        Assert.NotNull(user);
        Assert.Equal(applicationUser.Id, user.GetId());
        Assert.Equal(applicationUser.UserName, user.GetUserName());
        Assert.Equal(applicationUser.Email, user.GetEmail());
        Assert.Equal(applicationUser.PhoneNumber, user.GetPhoneNumber()!);
        Assert.Equal(applicationUser.PasswordHash, user.GetPasswordHash());
        Assert.Equal(roles, user.GetRoles());
    }

    [Fact]
    public void ShouldConvertApplicationUserToUserGivenNoPhoneNumberNoPasswordAndNoRoles()
    {
        ApplicationUser applicationUser =
            ApplicationUser.Builder()
            .WithId("8sIi8NZcD34l")
            .WithUserName("Jame")
            .WithEmail("jame@auth.com")
            .Build();

        User user = applicationUser.ToUser(null!);

        Assert.NotNull(user);
        Assert.Equal(applicationUser.Id, user.GetId());
        Assert.Equal(applicationUser.UserName, user.GetUserName());
        Assert.Equal(applicationUser.Email, user.GetEmail());
        Assert.Null(user.GetPhoneNumber());
        Assert.Null(user.GetPasswordHash());
        Assert.Null(user.GetRoles());
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullIdConvertingApplicationUserToUser()
    {
        ApplicationUser applicationUser =
            ApplicationUser.Builder()
            .WithId(null!)
            .WithUserName("Jame")
            .WithEmail("jame@auth.com")
            .WithPhoneNumber("111-222-3333")
            .WithPasswordHash("tOcR1%oH6F0B")
            .Build();
        string[] roles = new[] { "Standard" };

        Assert.Throws<ArgumentNullException>(() => applicationUser.ToUser(roles));
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenEmptyIdConvertingApplicationUserToUser()
    {
        ApplicationUser applicationUser =
            ApplicationUser.Builder()
            .WithId(string.Empty)
            .WithUserName("Jame")
            .WithEmail("jame@auth.com")
            .WithPhoneNumber("111-222-3333")
            .WithPasswordHash("tOcR1%oH6F0B")
            .Build();
        string[] roles = new[] { "Standard" };

        Assert.Throws<ArgumentException>(() => applicationUser.ToUser(roles));
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullUserNameConvertingApplicationUserToUser()
    {
        ApplicationUser applicationUser =
            ApplicationUser.Builder()
            .WithId("8sIi8NZcD34l")
            .WithEmail("jame@auth.com")
            .WithPhoneNumber("111-222-3333")
            .WithPasswordHash("tOcR1%oH6F0B")
            .Build();
        string[] roles = new[] { "Standard" };

        Assert.Throws<ArgumentNullException>(() => applicationUser.ToUser(roles));
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenEmptyUserNameConvertingApplicationUserToUser()
    {
        ApplicationUser applicationUser =
            ApplicationUser.Builder()
            .WithId("8sIi8NZcD34l")
            .WithUserName(string.Empty)
            .WithEmail("jame@auth.com")
            .WithPhoneNumber("111-222-3333")
            .WithPasswordHash("tOcR1%oH6F0B")
            .Build();
        string[] roles = new[] { "Standard" };

        Assert.Throws<ArgumentException>(() => applicationUser.ToUser(roles));
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullEmailConvertingApplicationUserToUser()
    {
        ApplicationUser applicationUser =
            ApplicationUser.Builder()
            .WithId("8sIi8NZcD34l")
            .WithPhoneNumber("111-222-3333")
            .WithPasswordHash("tOcR1%oH6F0B")
            .Build();
        string[] roles = new[] { "Standard" };

        Assert.Throws<ArgumentNullException>(() => applicationUser.ToUser(roles));
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenEmptyEmailConvertingApplicationUserToUser()
    {
        ApplicationUser applicationUser =
            ApplicationUser.Builder()
            .WithId("8sIi8NZcD34l")
            .WithUserName("James")
            .WithEmail(string.Empty)
            .WithPhoneNumber("111-222-3333")
            .WithPasswordHash("tOcR1%oH6F0B")
            .Build();
        string[] roles = new[] { "Standard" };

        Assert.Throws<ArgumentException>(() => applicationUser.ToUser(roles));
    }
}
