namespace BMJ.Authenticator.Infrastructure.Identity.Builders;

public class UserIdentificationBuilder : IUserIdentificationBuilder
{
    private string _id = null!;
    private string _username = null!;
    private string _email = null!;
    private string _phoneNumber = null!;
    private string[] _roles = null!;

    public UserIdentification Build() => new()
    { 
        Id = _id,
        UserName = _username,
        Email = _email,
        PhoneNumber = _phoneNumber,
        Roles = _roles
    };

    public IUserIdentificationBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public IUserIdentificationBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public IUserIdentificationBuilder WithPhoneNumber(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
        return this;
    }

    public IUserIdentificationBuilder WithUserName(string userName)
    {
        _username = userName;
        return this;
    }

    public IUserIdentificationBuilder WithRoles(string[] roles)
    {
        _roles = roles;
        return this;
    }
}
