using BMJ.Authenticator.Api.Filters;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace BMJ.Authenticator.Presentation
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services
                .AddProblemDetails()
                .AddEndpointsApiExplorer()
                .AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters()
                .AddMvcCore(options =>
                    options.Filters.Add<ApiExceptionFilterAttribute>())
                .AddApplicationPart(typeof(ConfigureServices).Assembly);
            return services;
        }
    }
}
