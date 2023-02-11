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
            .AddMvcCore(options =>
            {
                options.Filters.Add<ApiExceptionFilterAttribute>();
                options.Filters.Add<ApiLogFilterAttribute>();
            })
            .AddApplicationPart(typeof(ConfigureServices).Assembly);
        return services;
    }
}
