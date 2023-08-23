namespace BMJ.Authenticator.Infrastructure.Events;

public class UserCreatedEvent : BaseEvent
{
    public UserCreatedEvent() : base(nameof(UserCreatedEvent))
    {
    }

    public string UserName { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string Password { get; set; }
}
