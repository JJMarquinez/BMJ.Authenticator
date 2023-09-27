using BMJ.Authenticator.Infrastructure.Events.Handlers.Strategies;

namespace BMJ.Authenticator.Infrastructure.Events.Handlers.Factories;

public interface IEventHandlerStrategyFactory
{
    EventHandlerStrategy GetStrategy(Type eventType);
}
