using BMJ.Authenticator.Infrastructure.Identity;

namespace BMJ.Authenticator.Infrastructure.UnitTests.Identity;

public class ApplicationUserExtensionsTests
{
    [Fact]
    public void ShouldConvertApplicationUserToUserIdentification() 
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

        UserIdentification user = applicationUser.ToUserIdentification(roles);

        Assert.Equal(applicationUser.Id, user.Id);
        Assert.Equal(applicationUser.UserName, user.UserName);
        Assert.Equal(applicationUser.Email, user.Email);
        Assert.Equal(applicationUser.PhoneNumber, user.PhoneNumber!);
        Assert.Equal(roles, user.Roles);
    }

    [Fact]
    public void ShouldConvertApplicationUserToUserIdentificationGivenNullPhoneNumber()
    {
        ApplicationUser applicationUser =
            ApplicationUser.Builder()
            .WithId("8sIi8NZcD34l")
            .WithUserName("Jame")
            .WithEmail("jame@auth.com")
            .WithPasswordHash("tOcR1%oH6F0B")
            .Build();
        string[] roles = new[] { "Standard" };

        UserIdentification user = applicationUser.ToUserIdentification(roles);

        Assert.Equal(applicationUser.Id, user.Id);
        Assert.Equal(applicationUser.UserName, user.UserName);
        Assert.Equal(applicationUser.Email, user.Email);
        Assert.Equal(roles, user.Roles);
        Assert.Null(user.PhoneNumber!);
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

        UserIdentification user = applicationUser.ToUserIdentification(null!);

        Assert.Equal(applicationUser.Id, user.Id);
        Assert.Equal(applicationUser.UserName, user.UserName);
        Assert.Equal(applicationUser.Email, user.Email);
        Assert.Null(user.PhoneNumber);
        Assert.Null(user.Roles);
    }

    [Fact]
    public void ShouldConvertApplicationUserToUserGivenNoPhoneNumberNoPasswordNoRolesAndNoEmail()
    {
        ApplicationUser applicationUser =
            ApplicationUser.Builder()
            .WithId("8sIi8NZcD34l")
            .WithUserName("Jame")
            .Build();

        UserIdentification user = applicationUser.ToUserIdentification(null!);

        Assert.Equal(applicationUser.Id, user.Id);
        Assert.Equal(applicationUser.UserName, user.UserName);
        Assert.Null(user.Email);
        Assert.Null(user.PhoneNumber);
        Assert.Null(user.Roles);
    }

    [Fact]
    public void ShouldConvertApplicationUserToUserGivenNoPhoneNumberNoPasswordNoRolesNoEmailAndNoUsername()
    {
        ApplicationUser applicationUser =
            ApplicationUser.Builder()
            .WithId("8sIi8NZcD34l")

            .Build();

        UserIdentification user = applicationUser.ToUserIdentification(null!);

        Assert.Equal(applicationUser.Id, user.Id);
        Assert.Null(user.UserName);
        Assert.Null(user.Email);
        Assert.Null(user.PhoneNumber);
        Assert.Null(user.Roles);
    }

    [Fact]
    public void ShouldConvertApplicationUserToUserGivenNoPhoneNumberNoPasswordNoRolesNoEmailNoUsernameAndNoId()
    {
        ApplicationUser applicationUser =
            ApplicationUser.Builder()
            .Build();

        UserIdentification user = applicationUser.ToUserIdentification(null!);

        Assert.NotNull(user.Id);
        Assert.Null(user.UserName);
        Assert.Null(user.Email);
        Assert.Null(user.PhoneNumber);
        Assert.Null(user.Roles);
    }
}
