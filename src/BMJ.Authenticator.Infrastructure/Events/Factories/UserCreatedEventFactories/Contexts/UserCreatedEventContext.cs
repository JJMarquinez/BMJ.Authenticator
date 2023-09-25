namespace BMJ.Authenticator.Infrastructure.Events.Factories.UserCreatedEventFactories.Contexts;

public class UserCreatedEventContext : BaseEventContext
{
    public string UserName { get; }
    public string Email { get; }
    public string? Phone { get; }
    public string Password { get; }

    public UserCreatedEventContext(string userName, string email, string? phone, string password, Guid id, int version) 
        : base(id, version, nameof(UserCreatedEvent)) 
        => (UserName, Email, Phone, Password) = (userName, email, phone, password);

}
