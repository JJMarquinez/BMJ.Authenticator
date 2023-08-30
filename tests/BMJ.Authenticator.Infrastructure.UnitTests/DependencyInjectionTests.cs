using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Infrastructure.Consumers;
using BMJ.Authenticator.Infrastructure.Handlers;
using BMJ.Authenticator.Infrastructure.Identity;
using BMJ.Authenticator.Infrastructure.Loggers;
using BMJ.Authenticator.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BMJ.Authenticator.Infrastructure.UnitTests;

public class DependencyInjectionTests
{
    private readonly IServiceCollection _serviceCollection;

    public DependencyInjectionTests()
    {
        _serviceCollection = new ServiceCollection();
        _serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());
        _serviceCollection.AddOutputCache();
        _serviceCollection.AddInfrastructureServices(new ConfigurationManager());
    }

    [Fact]
    public void ShouldAddInfrastructureServices()
    {
        var serviceProvider = _serviceCollection.BuildServiceProvider();

        Assert.IsType<IdentityService>(serviceProvider.GetService<IIdentityService>());
        Assert.IsType<AuthLogger>(serviceProvider.GetService<IAuthLogger>());
        Assert.IsType<Infrastructure.Handlers.EventHandler>(serviceProvider.GetService<IEventHandler>());
        Assert.IsType<EventConsumer>(serviceProvider.GetService<IEventConsumer>());
        Assert.IsType<UserValidator<ApplicationUser>>(serviceProvider.GetService<IUserValidator<ApplicationUser>>());
    }

    [Fact]
    public void ShouldAddIdentityServices()
    {
        var serviceProvider = _serviceCollection.BuildServiceProvider();

        Assert.IsType<UserValidator<ApplicationUser>>(serviceProvider.GetService<IUserValidator<ApplicationUser>>());
        Assert.IsType<PasswordValidator<ApplicationUser>>(serviceProvider.GetService<IPasswordValidator<ApplicationUser>>());
        Assert.IsType<PasswordHasher<ApplicationUser>>(serviceProvider.GetService<IPasswordHasher<ApplicationUser>>());
        Assert.IsType<UpperInvariantLookupNormalizer>(serviceProvider.GetService<ILookupNormalizer>());
        Assert.IsType<DefaultUserConfirmation<ApplicationUser>>(serviceProvider.GetService<IUserConfirmation<ApplicationUser>>());
        Assert.NotNull(serviceProvider.GetService<UserManager<ApplicationUser>>());
        Assert.NotNull(serviceProvider.GetService<IdentityErrorDescriber>());
    }

    [Fact]
    public void ShouldAddDatabaseServices()
    {
        var serviceProvider = _serviceCollection.BuildServiceProvider();

        Assert.NotNull(serviceProvider.GetService<ApplicationDbContextInitialiser>());
        Assert.NotNull(serviceProvider.GetService<ApplicationDbContext>());
    }
}
