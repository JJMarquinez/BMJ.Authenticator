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
                .AddMvcCore()
                .AddApplicationPart(typeof(ConfigureServices).Assembly);
            return services;
        }
    }
}
