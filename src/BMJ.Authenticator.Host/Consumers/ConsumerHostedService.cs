using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Infrastructure.Consumers;

namespace BMJ.Authenticator.Host.Consumers;

public class ConsumerHostedService : IHostedService
{
    private readonly IAuthLogger _logger;
    private readonly IServiceProvider _serviceProvider;

    public ConsumerHostedService(IAuthLogger logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.Information("Event consumer service running.");

        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
            var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");

            Task.Run(() => eventConsumer.Consume(topic!), cancellationToken);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.Information("Event consumer service stopped.");
        return Task.CompletedTask;
    }
}
