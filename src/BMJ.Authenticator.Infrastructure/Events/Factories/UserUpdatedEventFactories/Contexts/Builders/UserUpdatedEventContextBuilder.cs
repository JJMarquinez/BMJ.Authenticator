namespace BMJ.Authenticator.Infrastructure.Events.Factories.UserUpdatedEventFactories.Contexts.Builders;

public class UserUpdatedEventContextBuilder : IUserUpdatedEventContextBuilder
{
    private Guid _id;
    private int _version;
    private string _username = null!;
    private string _email = null!;
    private string? _phone;

    public UserUpdatedEventContext Build()
        => new(_username, _email, _phone, _id, _version);

    public IUserUpdatedEventContextBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public IUserUpdatedEventContextBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public IUserUpdatedEventContextBuilder WithPhone(string phone)
    {
        _phone = phone;
        return this;
    }

    public IUserUpdatedEventContextBuilder WithUsername(string username)
    {
        _username = username;
        return this;
    }

    public IUserUpdatedEventContextBuilder WithVersion(int version)
    {
        _version = version;
        return this;
    }
}
