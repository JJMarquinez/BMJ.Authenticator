using AutoMapper;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Errors.Builders;
using BMJ.Authenticator.Application.Common.Models.Results.Builders;
using BMJ.Authenticator.Application.Common.Models.Users.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers.Factories;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById.Factories;
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

        Assert.IsType<UpdateUserCommandBuilder>(serviceProvider.GetService<IUpdateUserCommandBuilder>());
        Assert.IsType<DeleteUserCommandBuilder>(serviceProvider.GetService<IDeleteUserCommandBuilder>());
        Assert.IsType<CreateUserCommandBuilder>(serviceProvider.GetService<ICreateUserCommandBuilder>());
        Assert.IsType<GetAllUserQueryFactory>(serviceProvider.GetService<IGetAllUserQueryFactory>());
        Assert.IsType<GetUserByIdQueryFactory>(serviceProvider.GetService<IGetUserByIdQueryFactory>());
        Assert.IsType<ResultDtoGenericBuilder>(serviceProvider.GetService<IResultDtoGenericBuilder>());
        Assert.IsType<ResultDtoBuilder>(serviceProvider.GetService<IResultDtoBuilder>());
        Assert.IsType<UserDtoBuilder>(serviceProvider.GetService<IUserDtoBuilder>());
        Assert.IsType<ErrorDtoBuilder>(serviceProvider.GetService<IErrorDtoBuilder>());
    }
}
