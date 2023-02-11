using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Text;
using BMJ.Authenticator.Infrastructure.Authentication;
using Serilog;
using Serilog.Sinks.Elasticsearch;

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
            webApplicationBuilder.Host.UseSerilog((context, configuration) => 
            {
                configuration.Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(context.Configuration["Elasticsearch:Url"]))
                    {
                        IndexFormat = $"BMJ.Authenticator-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                        AutoRegisterTemplate = true,
                        NumberOfShards = 2,
                        NumberOfReplicas = 1
                    })
                .Enrich.WithProperty("Enviroment", context.HostingEnvironment.EnvironmentName)
                .ReadFrom.Configuration(context.Configuration);
            });
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
