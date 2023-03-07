using BMJ.Authenticator.Api.Caching;
using BMJ.Authenticator.Api.Filters;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BMJ.Authenticator.Api;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddProblemDetails()
            .AddEndpointsApiExplorer()
            .AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters()
            .AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]))
            .AddRedisOutputCache(options =>
            {
                options
                .AddAuthenticatorBaseCachePolicy()
                .AddByIdCachePolicy()
                .AddTokenCachePolicy();
            })
            .AddMvcCore(options =>
            {
                options.Filters.Add<ApiLogFilterAttribute>();
                options.Filters.Add<ApiExceptionFilterAttribute>();
                options.Filters.Add<AuthenticatorResultFilterAttribute>();
            })
            .AddApplicationPart(typeof(ConfigureServices).Assembly);
        return services;
    }
}
