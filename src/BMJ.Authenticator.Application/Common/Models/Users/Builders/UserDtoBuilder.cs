namespace BMJ.Authenticator.Application.Common.Models.Users.Builders;

public class UserDtoBuilder : IUserDtoBuilder
{
    private string _id = null!;
    private string _userName = null!;
    private string _email = null!;
    private string _phone = null!;
    private string[] _roles;

    public UserDto Build()
        => new()
        {
            Id = _id,
            UserName = _userName,
            Email = _email,
            PhoneNumber = _phone,
            Roles = _roles
        };

    public IUserDtoBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public IUserDtoBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public IUserDtoBuilder WithName(string name)
    {
        _userName = name;
        return this;
    }

    public IUserDtoBuilder WithPhone(string phone)
    {
        _phone = phone;
        return this;
    }

    public IUserDtoBuilder WithRoles(string[] roles)
    {
        _roles = roles;
        return this;
    }
}
