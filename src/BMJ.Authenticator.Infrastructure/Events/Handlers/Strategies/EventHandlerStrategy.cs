namespace BMJ.Authenticator.Infrastructure.Events.Handlers.Strategies;

public abstract class EventHandlerStrategy
{
    public abstract Task HandlerAsync(BaseEvent @event);

    public abstract bool Support(Type eventType);
}
