namespace BMJ.Authenticator.Infrastructure.Events.Factories.UserCreatedEventFactories.Contexts.Builders;

public class UserCreatedEventContextBuilder : IUserCreatedEventContextBuilder
{
    private Guid _id;
    private int _version;
    private string _username = null!;
    private string _password = null!;
    private string _email = null!;
    private string? _phone;

    public UserCreatedEventContext Build()
        => new(_username, _email, _phone, _password, _id, _version);

    public IUserCreatedEventContextBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public IUserCreatedEventContextBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public IUserCreatedEventContextBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public IUserCreatedEventContextBuilder WithPhone(string phone)
    {
        _phone = phone;
        return this;
    }

    public IUserCreatedEventContextBuilder WithUsername(string username)
    {
        _username = username;
        return this;
    }

    public IUserCreatedEventContextBuilder WithVersion(int version)
    {
        _version = version;
        return this;
    }
}
