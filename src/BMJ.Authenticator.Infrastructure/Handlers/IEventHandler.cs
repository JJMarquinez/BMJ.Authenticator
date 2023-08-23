using BMJ.Authenticator.Infrastructure.Events;

namespace BMJ.Authenticator.Infrastructure.Handlers;

public interface IEventHandler
{
    Task On(UserCreatedEvent @event);
    Task On(UserUpdatedEvent @event);
    Task On(UserDeletedEvent @event);
}
