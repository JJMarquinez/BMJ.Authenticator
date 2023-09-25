using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Errors.Builders;
using BMJ.Authenticator.Application.Common.Models.Results.FactoryMethods;
using BMJ.Authenticator.Infrastructure.Consumers;
using BMJ.Authenticator.Infrastructure.Events.Factories;
using BMJ.Authenticator.Infrastructure.Events.Factories.Creators;
using BMJ.Authenticator.Infrastructure.Events.Factories.UserCreatedEventFactories.Contexts.Builders;
using BMJ.Authenticator.Infrastructure.Events.Factories.UserDeletedEventFactories.Contexts.Builders;
using BMJ.Authenticator.Infrastructure.Events.Factories.UserUpdatedEventFactories.Contexts.Builders;
using BMJ.Authenticator.Infrastructure.Handlers;
using BMJ.Authenticator.Infrastructure.Identity;
using BMJ.Authenticator.Infrastructure.Identity.Builders;
using BMJ.Authenticator.Infrastructure.Loggers;
using BMJ.Authenticator.Infrastructure.Persistence;
using Confluent.Kafka;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace BMJ.Authenticator.Infrastructure.UnitTests;

public class DependencyInjectionTests
{
    private readonly IServiceCollection _serviceCollection;

    public DependencyInjectionTests()
    {
        _serviceCollection = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        _serviceCollection.AddTransient<IResultDtoFactory, ResultDtoFactory>();
        _serviceCollection.AddTransient<IResultDtoGenericFactory, ResultDtoGenericFactory>();
        _serviceCollection.AddTransient<IResultDtoCreator, ResultDtoCreator>();
        _serviceCollection.AddTransient<IErrorDtoBuilder, ErrorDtoBuilder>();
        _serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());
        _serviceCollection.AddOutputCache();
        _serviceCollection.AddInfrastructureServices(configuration);
    }

    [Fact]
    public void ShouldAddInfrastructureServices()
    {
        var serviceProvider = _serviceCollection.BuildServiceProvider();

        Assert.IsType<EventCreator>(serviceProvider.GetService<IEventCreator>());
        Assert.IsType<UserUpdatedEventContextBuilder>(serviceProvider.GetService<IUserUpdatedEventContextBuilder>());
        Assert.IsType<UserCreatedEventContextBuilder>(serviceProvider.GetService<IUserCreatedEventContextBuilder>());
        Assert.IsType<UserDeletedEventContextBuilder>(serviceProvider.GetService<IUserDeletedEventContextBuilder>());
        Assert.IsType<UserDeletedEventContextBuilder>(serviceProvider.GetService<IUserDeletedEventContextBuilder>());
        Assert.IsType<UserIdentificationBuilder>(serviceProvider.GetService<IUserIdentificationBuilder>());
        Assert.IsType<ApplicationUserBuilder>(serviceProvider.GetService<IApplicationUserBuilder>());
        Assert.IsType<IdentityService>(serviceProvider.GetService<IIdentityService>());
        Assert.IsType<AuthLogger>(serviceProvider.GetService<IAuthLogger>());
        Assert.IsType<Infrastructure.Handlers.EventHandler>(serviceProvider.GetService<IEventHandler>());
        Assert.IsType<EventConsumer>(serviceProvider.GetService<IEventConsumer>());
        Assert.IsType<UserValidator<ApplicationUser>>(serviceProvider.GetService<IUserValidator<ApplicationUser>>());
        Assert.NotNull(serviceProvider.GetService<IOptions<ConsumerConfig>>());
        Assert.Equal(3, serviceProvider.GetServices<EventFactory>().Count());
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
