using BMJ.Authenticator.Domain.ValueObjects;

namespace BMJ.Authenticator.Domain.Entities.Users.Builders;

public class UserBuilder : IUserBuilder, IUserNameBuilder, IUserEmailBuilder, IUserRolesPhonePasswordBuilder
{
    private string _id = null!;
    private string _userName = null!;
    private Email _email = null!;
    private Phone? _phone = null!;
    private string[]? _roles;

    private UserBuilder() { }

    internal static UserBuilder NewInstance() => new();

    public User Build() => User.NewInstance(_id, _userName, _email, _roles, _phone);

    public IUserRolesPhonePasswordBuilder WithEmail(Email email)
    {
        _email = email;
        return this;
    }

    public IUserNameBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public IUserEmailBuilder WithName(string name)
    {
        _userName = name;
        return this;
    }

    public IUserRolesPhonePasswordBuilder WithPhone(Phone phone)
    {
        _phone = phone;
        return this;
    }

    public IUserRolesPhonePasswordBuilder WithRoles(string[] roles)
    {
        _roles = roles;
        return this;
    }
}
