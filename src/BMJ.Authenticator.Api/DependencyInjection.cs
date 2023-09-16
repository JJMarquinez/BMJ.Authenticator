using BMJ.Authenticator.Api.Caching;
using BMJ.Authenticator.Api.Filters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BMJ.Authenticator.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
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
            .AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters()
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
                options.Filters.Add<AuthenticatorResultFilterAttribute>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            })
            .AddApiExplorer()
            .AddApplicationPart(typeof(DependencyInjection).Assembly);
        return services;
    }
}
