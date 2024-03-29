﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Text;
using BMJ.Authenticator.Adapter.Authentication;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using BMJ.Authenticator.Application.Common.Instrumentation;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using BMJ.Authenticator.Host.Consumers;

namespace BMJ.Authenticator.Host
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddHostServices(this IServiceCollection services, WebApplicationBuilder webApplicationBuilder)
        {
            services
                .AddCustomConfigure(webApplicationBuilder.Configuration)
                .AddCustomLogging(webApplicationBuilder)
                .AddCustomAuthentication(webApplicationBuilder.Configuration)
                .AddCustomOpenApiDocument()
                .AddCustomOpenTelemetry(webApplicationBuilder.Configuration)
                .AddCustomHealthChecks(webApplicationBuilder.Configuration)
                .AddHostedService<ConsumerHostedService>();
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
                    new ElasticsearchSinkOptions(new Uri(context.Configuration.GetValue<string>("Elasticsearch:Url")!))
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
            services.AddOpenApiDocument(configure => {
                configure.PostProcess = document =>
                {
                    document.Info = new OpenApiInfo
                    {
                        Title = "BMJ Query Authenticator API",
                        Description = "An ASP.NET Core Web API to query identity users' detials and get tokens",
                        TermsOfService = "https://example.com/terms",
                        Contact = new OpenApiContact
                        {
                            Name = "IT Team",
                            Url = "https://example.com/contact"
                        },
                        License = new OpenApiLicense
                        {
                            Name = "IT License",
                            Url = "https://example.com/license"
                        }
                    };
                };

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
                        ValidIssuer = configuration.GetValue<string>("JwtOptions:Issuer"),
                        ValidAudience = configuration.GetValue<string>("JwtOptions:Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration.GetValue<string>("JwtOptions:SecretKey")!)
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

        private static IServiceCollection AddCustomOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenTelemetry().WithTracing(options =>
            {
                options
                .AddSource(Telemetry.ServiceName)
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName: Telemetry.ServiceName, serviceVersion: Telemetry.Version).AddTelemetrySdk())
                .AddSqlClientInstrumentation(options =>
                {
                    options.SetDbStatementForText = true;
                    options.SetDbStatementForStoredProcedure = true;
                    options.RecordException = true;
                })
                .AddAspNetCoreInstrumentation()
                .AddRedisInstrumentation()
                .AddElasticsearchClientInstrumentation()
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(configuration.GetValue<string>("OtlpExporter:Endpoint")!);
                });
            });
            return services;
        }

        private static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHealthChecks()
                .AddSqlServer(configuration.GetValue<string>("ConnectionStrings:DefaultConnection")!)
                .AddRedis(configuration.GetValue<string>("Redis:Configuration")!)
                .AddElasticsearch(configuration.GetValue<string>("Elasticsearch:Url")!);
            services.AddHealthChecksUI().AddInMemoryStorage();
            return services;
        }
    }
}
