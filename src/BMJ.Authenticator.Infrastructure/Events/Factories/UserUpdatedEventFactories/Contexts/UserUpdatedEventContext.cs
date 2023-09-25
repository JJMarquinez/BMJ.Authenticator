namespace BMJ.Authenticator.Infrastructure.Events.Factories.UserUpdatedEventFactories.Contexts;

public class UserUpdatedEventContext : BaseEventContext
{
    public string UserName { get; }
    public string Email { get; }
    public string? Phone { get; }

    public UserUpdatedEventContext(string userName, string email, string? phone, Guid id, int version)
        : base(id, version, nameof(UserUpdatedEvent))
        => (UserName, Email, Phone) = (userName, email, phone);
}
