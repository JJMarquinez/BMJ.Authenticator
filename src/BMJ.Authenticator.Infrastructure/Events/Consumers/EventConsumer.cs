using BMJ.Authenticator.Infrastructure.Converters;
using BMJ.Authenticator.Infrastructure.Events.Handlers;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace BMJ.Authenticator.Infrastructure.Events.Consumers;

public class EventConsumer : IEventConsumer
{
    private readonly ConsumerConfig _config;
    private readonly IEventHandlerStrategyContext _eventHandlerStrategyContext;
    public EventConsumer(IOptions<ConsumerConfig> config, IEventHandlerStrategyContext eventHandlerStrategyContext)
    {
        _config = config.Value;
        _eventHandlerStrategyContext = eventHandlerStrategyContext;
    }

    public async Task Consume(string topic)
    {
        using var consumer = new ConsumerBuilder<string, string>(_config)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(Deserializers.Utf8)
            .Build();

        consumer.Subscribe(topic);

        while (true)
        {
            var consumeResult = consumer.Consume();

            if (consumeResult?.Message == null) continue;

            var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
            var @event = JsonSerializer.Deserialize<BaseEvent>(consumeResult.Message.Value, options);
            await _eventHandlerStrategyContext.ExecuteHandlingAsync(@event!);
            consumer.Commit(consumeResult);
        }
    }
}
