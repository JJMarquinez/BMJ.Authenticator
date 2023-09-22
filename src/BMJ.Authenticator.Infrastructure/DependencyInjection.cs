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

            services.AddTransient<IUserIdentificationBuilder, UserIdentificationBuilder>();
            services.AddTransient<IApplicationUserBuilder, ApplicationUserBuilder>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IAuthLogger, AuthLogger>();
            services.AddTransient<IEventHandler, Handlers.EventHandler>();
            services.AddTransient<IEventConsumer, EventConsumer>();
            services.Configure<ConsumerConfig>(configuration.GetSection(nameof(ConsumerConfig)));

            return services;
        }
    }
}
