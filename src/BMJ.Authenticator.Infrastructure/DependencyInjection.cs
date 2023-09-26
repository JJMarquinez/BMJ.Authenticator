using BMJ.Authenticator.Infrastructure.Identity;
using BMJ.Authenticator.Infrastructure.Loggers;
using BMJ.Authenticator.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Infrastructure.Handlers;
using BMJ.Authenticator.Infrastructure.Consumers;
using Confluent.Kafka;
using BMJ.Authenticator.Infrastructure.Identity.Builders;
using BMJ.Authenticator.Infrastructure.Events.Factories.Creators;
using System.Reflection;
using BMJ.Authenticator.Infrastructure.Events.Factories;
using BMJ.Authenticator.Infrastructure.Events.Factories.UserDeletedEventFactories.Contexts.Builders;
using BMJ.Authenticator.Infrastructure.Events.Factories.UserCreatedEventFactories.Contexts.Builders;
using BMJ.Authenticator.Infrastructure.Events.Factories.UserUpdatedEventFactories.Contexts.Builders;

namespace BMJ.Authenticator.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<ApplicationDbContextInitialiser>();
            
            services
                .AddAuthorization()
                .AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddEventFactories()
                .AddTransient<IEventCreator, EventCreator>()
                .AddTransient<IUserUpdatedEventContextBuilder, UserUpdatedEventContextBuilder>()
                .AddTransient<IUserCreatedEventContextBuilder, UserCreatedEventContextBuilder>()
                .AddTransient<IUserDeletedEventContextBuilder, UserDeletedEventContextBuilder>()
                .AddTransient<IUserIdentificationBuilder, UserIdentificationBuilder>()
                .AddTransient<IApplicationUserBuilder, ApplicationUserBuilder>()
                .AddTransient<IIdentityService, IdentityService>()
                .AddTransient<IAuthLogger, AuthLogger>()
                .AddTransient<IEventHandler, Handlers.EventHandler>()
                .AddTransient<IEventConsumer, EventConsumer>()
                .Configure<ConsumerConfig>(configuration.GetSection(nameof(ConsumerConfig)));

            return services;
        }

        private static IServiceCollection AddEventFactories(this IServiceCollection services)
        {
            var types = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition && typeof(EventFactory).IsAssignableFrom(type));

            types.ToList().ForEach(type => services.AddTransient(typeof(EventFactory), type));
            return services;
        }
    }
}
