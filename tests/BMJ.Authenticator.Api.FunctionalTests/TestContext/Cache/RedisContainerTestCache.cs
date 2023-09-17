using Testcontainers.Redis;

namespace BMJ.Authenticator.Api.FunctionalTests.TestContext.Cache;

public class RedisContainerTestCache : ITestCache
{
    private readonly RedisContainer _redisContainer;
    private string _redisConnectionString = null!;

    public RedisContainerTestCache()
    {
        _redisContainer = new RedisBuilder().Build();
    }

    public async ValueTask DisposeAsync()
    {
        await _redisContainer.DisposeAsync().ConfigureAwait(false);
    }

    public string GetConnectionString() => _redisConnectionString;

    public async Task InitialiseAsync()
    {
        await _redisContainer.StartAsync().ConfigureAwait(false);
        _redisConnectionString = _redisContainer.GetConnectionString();
    }
}
