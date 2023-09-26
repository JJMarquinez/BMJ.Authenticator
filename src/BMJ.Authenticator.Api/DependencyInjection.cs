using BMJ.Authenticator.Api.Caching;
using BMJ.Authenticator.Api.Exceptions.Strategies;
using BMJ.Authenticator.Api.Exceptions.Strategies.Factories;
using BMJ.Authenticator.Api.Exceptions.Strategies.Handlers;
using BMJ.Authenticator.Api.Exceptions.Strategies.Handlers.Supporters;
using BMJ.Authenticator.Api.Filters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Reflection;

namespace BMJ.Authenticator.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddExceptionHandlers()
            .AddScoped<IExceptionSupporter, ExceptionSupporter>()
            .AddScoped<IExceptionHandlerStrategyFactory, ExceptionHandlerStrategyFactory>()
            .AddScoped<IExceptionHandlerStrategyContext, ExceptionHandlerStrategyContext>()
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                                new HeaderApiVersionReader("x-api-version"),
                                                                new MediaTypeApiVersionReader("x-api-version"));
            })
            .AddProblemDetails()
            .AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(configuration.GetValue<string>("Redis:Configuration")!))
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
                options.Filters.Add<ApiResultFilterAttribute>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            })
            .AddApiExplorer()
            .AddApplicationPart(typeof(DependencyInjection).Assembly);
        return services;
    }

    private static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
    {
        var types = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition && typeof(ExceptionHandlerStrategy).IsAssignableFrom(type));

        types.ToList().ForEach(type => services.AddScoped(typeof(ExceptionHandlerStrategy), type));
        return services;
    }
}
