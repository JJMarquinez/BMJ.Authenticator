using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Behaviours;
using BMJ.Authenticator.Application.Common.Models.Errors.Builders;
using BMJ.Authenticator.Application.Common.Models.Results.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser.Builders;
using BMJ.Authenticator.Infrastructure.Events.Consumers;
using BMJ.Authenticator.Infrastructure.Events.Factories;
using BMJ.Authenticator.Infrastructure.Events.Factories.Creators;
using BMJ.Authenticator.Infrastructure.Events.Factories.UserCreatedEventFactories.Contexts.Builders;
using BMJ.Authenticator.Infrastructure.Events.Factories.UserDeletedEventFactories.Contexts.Builders;
using BMJ.Authenticator.Infrastructure.Events.Factories.UserUpdatedEventFactories.Contexts.Builders;
using BMJ.Authenticator.Infrastructure.Events.Handlers;
using BMJ.Authenticator.Infrastructure.Events.Handlers.Factories;
using BMJ.Authenticator.Infrastructure.Events.Handlers.Strategies;
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
        _serviceCollection.AddTransient<ICreateUserCommandBuilder, CreateUserCommandBuilder>();
        _serviceCollection.AddTransient<IDeleteUserCommandBuilder, DeleteUserCommandBuilder>();
        _serviceCollection.AddTransient<IUpdateUserCommandBuilder, UpdateUserCommandBuilder>();
        _serviceCollection.AddTransient<IResultDtoBuilder, ResultDtoBuilder>();
        _serviceCollection.AddTransient<IResultDtoGenericBuilder, ResultDtoGenericBuilder>();
        _serviceCollection.AddTransient<IErrorDtoBuilder, ErrorDtoBuilder>();
        _serviceCollection.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
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
        Assert.IsType<ApiLogger>(serviceProvider.GetService<IApiLogger>());
        Assert.IsType<EventHandlerStrategyFactory>(serviceProvider.GetService<IEventHandlerStrategyFactory>());
        Assert.IsType<EventHandlerStrategyContext>(serviceProvider.GetService<IEventHandlerStrategyContext>());
        Assert.IsType<EventConsumer>(serviceProvider.GetService<IEventConsumer>());
        Assert.IsType<UserValidator<ApplicationUser>>(serviceProvider.GetService<IUserValidator<ApplicationUser>>());
        Assert.NotNull(serviceProvider.GetService<IOptions<ConsumerConfig>>());
        Assert.Equal(3, serviceProvider.GetServices<EventFactory>().Count());
        Assert.Equal(3, serviceProvider.GetServices<EventHandlerStrategy>().Count());
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
