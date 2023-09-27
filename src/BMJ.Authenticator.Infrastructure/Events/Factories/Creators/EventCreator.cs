using BMJ.Authenticator.Infrastructure.Properties;

namespace BMJ.Authenticator.Infrastructure.Events.Factories.Creators;

public class EventCreator : IEventCreator
{
    private readonly IReadOnlyList<EventFactory> _eventFactories;

    public EventCreator(IEnumerable<EventFactory> eventFactories)
    {
        _eventFactories = eventFactories.ToList().AsReadOnly();
    }

    public BaseEvent Create(BaseEventContext context)
    {
        var factory = GetEventFactory(context.GetType());
        return factory.Make(context);
    }

    private EventFactory GetEventFactory(Type eventContextType)
    {
        EventFactory factory = null!;
        var factories = _eventFactories.Where(x => x.Support(eventContextType)).ToList();

        if (factories.Count == 1)
            factory = factories.First();
        else
            throw new Exception(InfrastructureString.AmbiguousOrNoOneEventFactory);

        return factory;
    }
}
