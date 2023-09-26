using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Infrastructure.Consumers;

namespace BMJ.Authenticator.Host.Consumers;

public class ConsumerHostedService : IHostedService
{
    private readonly IApiLogger _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ConsumerHostedService(IApiLogger logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.Information("Event consumer service running.");

        using (IServiceScope scope = _serviceScopeFactory.CreateScope())
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
