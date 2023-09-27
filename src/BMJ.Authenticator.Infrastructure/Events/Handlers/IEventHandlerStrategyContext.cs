namespace BMJ.Authenticator.Infrastructure.Events.Handlers;

public interface IEventHandlerStrategyContext
{
    Task ExecuteHandlingAsync(BaseEvent @event);
}
