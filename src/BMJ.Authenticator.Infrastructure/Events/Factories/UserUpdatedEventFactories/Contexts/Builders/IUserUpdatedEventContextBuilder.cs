namespace BMJ.Authenticator.Infrastructure.Events.Factories.UserUpdatedEventFactories.Contexts.Builders;

public interface IUserUpdatedEventContextBuilder
{
    public IUserUpdatedEventContextBuilder WithId(Guid id);
    public IUserUpdatedEventContextBuilder WithVersion(int version);
    public IUserUpdatedEventContextBuilder WithUsername(string username);
    public IUserUpdatedEventContextBuilder WithEmail(string email);
    public IUserUpdatedEventContextBuilder WithPhone(string phone);
    public UserUpdatedEventContext Build();
}
