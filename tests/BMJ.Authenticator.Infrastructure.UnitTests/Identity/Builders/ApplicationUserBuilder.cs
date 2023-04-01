using BMJ.Authenticator.Infrastructure.Identity;

namespace BMJ.Authenticator.Infrastructure.UnitTests.Identity.Builders;

internal class ApplicationUserBuilder : IApplicationUserBuilder
{
    private ApplicationUser _applicationUser = new ApplicationUser();
    private ApplicationUserBuilder() { }

    public static ApplicationUserBuilder New() => new ApplicationUserBuilder();

    public ApplicationUser Build() => _applicationUser;

    public IApplicationUserBuilder WithEmail(string email)
    {
        _applicationUser.Email = email;
        return this;
    }

    public IApplicationUserBuilder WithId(string id)
    {
        _applicationUser.Id = id;
        return this;
    }

    public IApplicationUserBuilder WithPasswordHash(string passwordHash)
    {
        _applicationUser.PasswordHash = passwordHash;
        return this;
    }

    public IApplicationUserBuilder WithPhoneNumber(string phoneNumber)
    {
        _applicationUser.PhoneNumber = phoneNumber;
        return this;
    }

    public IApplicationUserBuilder WithUserName(string userName)
    {
        _applicationUser.UserName = userName;
        return this;
    }
}
