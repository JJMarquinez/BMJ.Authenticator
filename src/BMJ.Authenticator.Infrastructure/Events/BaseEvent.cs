using BMJ.Authenticator.Infrastructure.Messages;

namespace BMJ.Authenticator.Infrastructure.Events;

public abstract class BaseEvent : Message
{
    protected BaseEvent(string type)
    {
        Type = type;
    }

    public int Version { get; set; }
    public string? Type { get; set; }
}
