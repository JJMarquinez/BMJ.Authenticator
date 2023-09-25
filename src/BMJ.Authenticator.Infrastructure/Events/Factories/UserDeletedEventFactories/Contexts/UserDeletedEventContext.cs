namespace BMJ.Authenticator.Infrastructure.Events.Factories.UserDeletedEventFactories.Contexts;

public class UserDeletedEventContext : BaseEventContext
{
    public UserDeletedEventContext(Guid id, int version)
        : base(id, version, nameof(UserDeletedEvent))
    {
    }
}
