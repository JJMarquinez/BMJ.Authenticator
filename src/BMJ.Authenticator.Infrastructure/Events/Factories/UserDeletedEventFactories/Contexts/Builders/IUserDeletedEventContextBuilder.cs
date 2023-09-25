namespace BMJ.Authenticator.Infrastructure.Events.Factories.UserDeletedEventFactories.Contexts.Builders;

public interface IUserDeletedEventContextBuilder
{
    public IUserDeletedEventContextBuilder WithId(Guid id);
    public IUserDeletedEventContextBuilder WithVersion(int version);
    public UserDeletedEventContext Build();
}
