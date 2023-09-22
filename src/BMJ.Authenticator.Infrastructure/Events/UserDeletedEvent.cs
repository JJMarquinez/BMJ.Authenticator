namespace BMJ.Authenticator.Infrastructure.Events;

public class UserDeletedEvent : BaseEvent
{
    public UserDeletedEvent() : base(nameof(UserDeletedEvent))
    {
    }
}
