using BMJ.Authenticator.Infrastructure.Events.Handlers.Strategies;
using BMJ.Authenticator.Infrastructure.Properties;

namespace BMJ.Authenticator.Infrastructure.Events.Handlers.Factories;

public class EventHandlerStrategyFactory : IEventHandlerStrategyFactory
{
    private readonly IReadOnlyList<EventHandlerStrategy> _handlerStrategies;

    public EventHandlerStrategyFactory(IEnumerable<EventHandlerStrategy> handlerStrategies)
    {
        _handlerStrategies = handlerStrategies.ToList().AsReadOnly();
    }

    public EventHandlerStrategy GetStrategy(Type eventType)
    {
        EventHandlerStrategy strategy = null!;
        var strategies = _handlerStrategies.Where(x => x.Support(eventType)).ToList();
        
        if (strategies.Count == 1)
            strategy = strategies.First();
        else
            throw new Exception(InfrastructureString.AmbiguousOrNoOneEventHandlerStrategy);

        return strategy;
    }
}
