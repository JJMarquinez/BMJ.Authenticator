using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;

namespace BMJ.Authenticator.Api.Caching;

public static class RedisOutputCacheServiceCollectionExtensions
{
    public static IServiceCollection AddRedisOutputCache(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services
            .AddOutputCache()
            .RemoveAll<IOutputCacheStore>()
            .AddSingleton<IOutputCacheStore, RedisOutputCacheStore>();
        return services;
    }

    public static IServiceCollection AddRedisOutputCache(this IServiceCollection services, Action<OutputCacheOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);

        services
            .Configure(configureOptions)
            .AddOutputCache()
            .RemoveAll<IOutputCacheStore>()
            .AddSingleton<IOutputCacheStore, RedisOutputCacheStore>(); ;

        return services;
    }
}
