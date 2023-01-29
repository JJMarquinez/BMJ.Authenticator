using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Text;
using BMJ.Authenticator.Infrastructure.Authentication;
using Serilog;
using Microsoft.AspNetCore.Builder;

namespace BMJ.Authenticator.Host
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddHostServices(this IServiceCollection services, WebApplicationBuilder webApplicationBuilder)
        {
            services
                .AddCustomConfigure(webApplicationBuilder.Configuration)
                .AddCustomLogging(webApplicationBuilder)
                .AddCustomAuthentication(webApplicationBuilder.Configuration)
                .AddCustomOpenApiDocument();
            return services;
        }

        private static IServiceCollection AddCustomLogging(this IServiceCollection services, WebApplicationBuilder webApplicationBuilder)
        {
            using var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            webApplicationBuilder.Host.UseSerilog(logger);
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

        private static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JwtOptions:Issuer"],
                        ValidAudience = configuration["JwtOptions:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["JwtOptions:SecretKey"])
                        )
                    };
                }
                );
            return services;
        }

        private static IServiceCollection AddCustomConfigure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
            return services;
        }
    }
}
