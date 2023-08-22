namespace BMJ.Authenticator.Infrastructure.Consumers;

public interface IEventConsumer
{
    void Consume(string topic);
}
