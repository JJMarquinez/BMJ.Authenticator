namespace BMJ.Authenticator.Infrastructure.Identity.Builders;

public class ApplicationUserBuilder : IApplicationUserBuilder
{
    private string _id = null!;
    private string _username = null!;
    private string _email = null!;
    private string _passwordHash = null!;
    private string _phoneNumber = null!;

    public ApplicationUser Build() => new()
    { 
        Id = _id ?? Guid.NewGuid().ToString(),
        UserName = _username,
        Email = _email,
        PasswordHash = _passwordHash,
        PhoneNumber = _phoneNumber
    };

    public IApplicationUserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public IApplicationUserBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public IApplicationUserBuilder WithPasswordHash(string passwordHash)
    {
        _passwordHash = passwordHash;
        return this;
    }

    public IApplicationUserBuilder WithPhoneNumber(string? phoneNumber)
    {
        _phoneNumber = phoneNumber;
        return this;
    }

    public IApplicationUserBuilder WithUserName(string userName)
    {
        _username = userName;
        return this;
    }
}
