namespace BMJ.Authenticator.Infrastructure.Events.Factories.UserCreatedEventFactories.Contexts.Builders;

public interface IUserCreatedEventContextBuilder
{
    public IUserCreatedEventContextBuilder WithId(Guid id);
    public IUserCreatedEventContextBuilder WithVersion(int version);
    public IUserCreatedEventContextBuilder WithUsername(string username);
    public IUserCreatedEventContextBuilder WithEmail(string email);
    public IUserCreatedEventContextBuilder WithPhone(string phone);
    public IUserCreatedEventContextBuilder WithPassword(string password);
    public UserCreatedEventContext Build();
}
