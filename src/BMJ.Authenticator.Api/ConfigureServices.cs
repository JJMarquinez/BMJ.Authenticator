using BMJ.Authenticator.Api.Caching;
using BMJ.Authenticator.Api.Filters;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace BMJ.Authenticator.Api;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services
            .AddProblemDetails()
            .AddEndpointsApiExplorer()
            .AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters()
            .AddOutputCache(options =>
            {
                options.AddPolicy(nameof(AuthenticatorBaseCachePolicy), builder => 
                {
                    builder.AddPolicy<AuthenticatorBaseCachePolicy>();
                    builder.Expire(TimeSpan.FromSeconds(86400));
                });

                options.AddPolicy(nameof(ByIdCachePolicy), builder =>
                {
                    builder.AddPolicy<ByIdCachePolicy>();
                    builder.Expire(TimeSpan.FromSeconds(86400));
                    builder.VaryByValue(ByIdCachePolicy.VaryByValue);
                });
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
