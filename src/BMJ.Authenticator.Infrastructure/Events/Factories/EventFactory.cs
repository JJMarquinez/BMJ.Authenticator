namespace BMJ.Authenticator.Infrastructure.Events.Factories;

public abstract class EventFactory
{
    public abstract BaseEvent Make(BaseEventContext context);

    public abstract bool Support(Type type);
}
