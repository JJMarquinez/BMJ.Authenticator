using Microsoft.AspNetCore.OutputCaching;
using StackExchange.Redis;

namespace BMJ.Authenticator.Api.Caching;

public class RedisOutputCacheStore : IOutputCacheStore
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisOutputCacheStore(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async ValueTask EvictByTagAsync(string tag, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(tag);

        IDatabase db = _connectionMultiplexer.GetDatabase();
        RedisValue[]? cacheKeys = await db.SetMembersAsync(tag);

        if(cacheKeys is not null && cacheKeys.Length > 0 )
        {
            RedisKey[] keys = cacheKeys
            .Select(k => (RedisKey)k.ToString())
            .Concat(new[] { (RedisKey)tag })
            .ToArray();

            await db.KeyDeleteAsync(keys);
        }
    }

    public async ValueTask<byte[]?> GetAsync(string key, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(key);
        IDatabase db = _connectionMultiplexer.GetDatabase();
        return await db.StringGetAsync(key);
    }

    public async ValueTask SetAsync(string key, byte[] value, string[]? tags, TimeSpan validFor, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);
    
        IDatabase db = _connectionMultiplexer.GetDatabase();
        if (tags is not null)
        {
            foreach (string tag in tags ?? Array.Empty<string>())
            {
                await db.SetAddAsync(tag, key);
            }
        }
        await db.StringSetAsync(key, value, validFor);
    }
}
