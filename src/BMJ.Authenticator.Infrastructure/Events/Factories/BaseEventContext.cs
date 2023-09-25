namespace BMJ.Authenticator.Infrastructure.Events.Factories;

public abstract class BaseEventContext
{
    public Guid Id { get; }
    public int Version { get; }
    public string? Type { get; }

    protected BaseEventContext(Guid id, int version, string? type)
    {
        Id = id;
        Version = version;
        Type = type;
    }
}
