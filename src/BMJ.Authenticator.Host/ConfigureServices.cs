using BMJ.Authenticator.App.OptionSetup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace BMJ.Authenticator.Host
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddHostServices(this IServiceCollection services)
        {
            services
                .AddCustomAuthentication()
                .AddCustomConfigure()
                .AddCustomOpenApiDocument();
            
            return services;
        }

        private static IServiceCollection AddCustomOpenApiDocument(this IServiceCollection services)
        {
            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "BMJ Authenticator API";
                configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });

                configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });
            return services;
        }

        private static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();
            return services;
        }

        private static IServiceCollection AddCustomConfigure(this IServiceCollection services)
        {
            services.ConfigureOptions<JwtOptionsSetup>();
            //services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
            services.ConfigureOptions<JwtBearerOptionsSetup>();
            return services;
        }
    }
}
