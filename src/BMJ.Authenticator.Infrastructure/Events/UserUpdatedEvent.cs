namespace BMJ.Authenticator.Infrastructure.Events;

public class UserUpdatedEvent : BaseEvent
{
    public UserUpdatedEvent() : base(nameof(UserUpdatedEvent))
    {
    }

    public string UserName { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
}
