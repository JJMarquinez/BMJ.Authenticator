namespace BMJ.Authenticator.Infrastructure.Events;

public class UserDeletedEvent : BaseEvent
{
    public UserDeletedEvent(string type) : base(nameof(UserDeletedEvent))
    {
    }

    public Guid UserId { get; set; }
}
