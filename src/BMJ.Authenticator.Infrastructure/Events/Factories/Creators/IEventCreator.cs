namespace BMJ.Authenticator.Infrastructure.Events.Factories.Creators;

public interface IEventCreator
{
    BaseEvent Create(BaseEventContext context);
}
