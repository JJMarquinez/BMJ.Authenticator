using AutoMapper;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Errors.Builders;
using BMJ.Authenticator.Application.Common.Models.Results.FactoryMethods;
using BMJ.Authenticator.Application.Common.Models.Users.Builders;
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

    [Fact]
    public void ShouldAddApplicationServices()
    {
        var serviceProvider = _serviceCollection.BuildServiceProvider();

        Assert.IsType<ResultDtoFactory>(serviceProvider.GetService<IResultDtoFactory>());
        Assert.IsType<ResultDtoGenericFactory>(serviceProvider.GetService<IResultDtoGenericFactory>());
        Assert.IsType<ResultDtoCreator>(serviceProvider.GetService<IResultDtoCreator>());
        Assert.IsType<UserDtoBuilder>(serviceProvider.GetService<IUserDtoBuilder>());
        Assert.IsType<ErrorDtoBuilder>(serviceProvider.GetService<IErrorDtoBuilder>());
    }
}
