using AutoMapper;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById;
using BMJ.Authenticator.Application.UseCases.Users.Queries.LoginUser;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BMJ.Authenticator.Application.UnitTests.DependencyInjection;

public class DependencyInjectionTests
{
    private readonly IServiceCollection _serviceCollection;
    public DependencyInjectionTests()
    {
        _serviceCollection = new ServiceCollection();
        _serviceCollection.AddTransient<IIdentityAdapter, IdentityAdapterTest>();
        _serviceCollection.AddApplicationServices();
    }

    [Fact]
    public void ShouldAddAutoMapperServices()
    {
        var serviceProvider = _serviceCollection.BuildServiceProvider();

        Assert.IsType<MapperConfiguration>(serviceProvider.GetService<IConfigurationProvider>());
        Assert.IsType<Mapper>(serviceProvider.GetService<IMapper>());
    }

    [Fact]
    public void ShouldAddValidators()
    {
        var serviceProvider = _serviceCollection.BuildServiceProvider();

        Assert.IsAssignableFrom<AbstractValidator<GetUserByIdQuery>>(serviceProvider.GetService<GetUserByIdQueryValidator>());
        Assert.IsAssignableFrom<AbstractValidator<LoginUserQuery>>(serviceProvider.GetService<LoginUserQueryValidator>());
    }

    [Fact]
    public void ShouldAddMediatorServices()
    {
        var serviceProvider = _serviceCollection.BuildServiceProvider();

        Assert.IsType<Mediator>(serviceProvider.GetService<IMediator>());
        Assert.IsType<Mediator>(serviceProvider.GetService<ISender>());
        Assert.IsType<Mediator>(serviceProvider.GetService<IPublisher>());
    }
}
