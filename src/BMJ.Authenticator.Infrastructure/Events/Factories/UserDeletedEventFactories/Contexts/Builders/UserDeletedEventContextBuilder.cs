namespace BMJ.Authenticator.Infrastructure.Events.Factories.UserDeletedEventFactories.Contexts.Builders;

public class UserDeletedEventContextBuilder : IUserDeletedEventContextBuilder
{
    private Guid _id;
    private int _version;

    public UserDeletedEventContext Build()
        => new(_id, _version);

    public IUserDeletedEventContextBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public IUserDeletedEventContextBuilder WithVersion(int version)
    {
        _version = version;
        return this;
    }
}
