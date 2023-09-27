using BMJ.Authenticator.Infrastructure.Events.Handlers.Factories;

namespace BMJ.Authenticator.Infrastructure.Events.Handlers;

public class EventHandlerStrategyContext : IEventHandlerStrategyContext
{
    private readonly IEventHandlerStrategyFactory _handlerStrategyFactory;

    public EventHandlerStrategyContext(IEventHandlerStrategyFactory handlerStrategyFactory)
    {
        _handlerStrategyFactory = handlerStrategyFactory;
    }

    public Task ExecuteHandlingAsync(BaseEvent @event)
    {
        var eventHandler = _handlerStrategyFactory.GetStrategy(@event.GetType());
        return eventHandler.HandlerAsync(@event);
    }
}
