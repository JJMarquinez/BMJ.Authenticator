namespace BMJ.Authenticator.Infrastructure.Events.Consumers;

public interface IEventConsumer
{
    Task Consume(string topic);
}
