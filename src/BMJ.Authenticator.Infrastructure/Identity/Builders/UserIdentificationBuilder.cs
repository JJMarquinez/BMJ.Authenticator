using BMJ.Authenticator.Domain.Entities.Users.Builders;

namespace BMJ.Authenticator.Infrastructure.Identity.Builders;

public class UserIdentificationBuilder : IUserIdentificationBuilder
{
    private UserIdentification _userIdentification = new UserIdentification();
    private UserIdentificationBuilder() { }

    internal static UserIdentificationBuilder New() => new UserIdentificationBuilder();

    public UserIdentification Build() => _userIdentification;

    public IUserIdentificationBuilder WithEmail(string email)
    {
        _userIdentification.Email = email;
        return this;
    }

    public IUserIdentificationBuilder WithId(string id)
    {
        _userIdentification.Id = id;
        return this;
    }

    public IUserIdentificationBuilder WithPhoneNumber(string? phoneNumber)
    {
        _userIdentification.PhoneNumber = phoneNumber;
        return this;
    }

    public IUserIdentificationBuilder WithUserName(string userName)
    {
        _userIdentification.UserName = userName;
        return this;
    }

    public IUserIdentificationBuilder WithRoles(string[]? roles)
    {
        _userIdentification.Roles = roles;
        return this;
    }
}
