using BMJ.Authenticator.Application.Common.Behaviours;
using BMJ.Authenticator.Application.Common.Models.Errors.Builders;
using BMJ.Authenticator.Application.Common.Models.Results.Builders;
using BMJ.Authenticator.Application.Common.Models.Users.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers.Factories;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById.Factories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BMJ.Authenticator.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services
                .AddTransient<IUpdateUserCommandBuilder, UpdateUserCommandBuilder>()
                .AddTransient<IDeleteUserCommandBuilder, DeleteUserCommandBuilder>()
                .AddTransient<ICreateUserCommandBuilder, CreateUserCommandBuilder>()
                .AddTransient<IGetAllUserQueryFactory, GetAllUserQueryFactory>()
                .AddTransient<IGetUserByIdQueryFactory, GetUserByIdQueryFactory>()
                .AddTransient<IResultDtoBuilder, ResultDtoBuilder>()
                .AddTransient<IResultDtoGenericBuilder, ResultDtoGenericBuilder>()
                .AddTransient<IUserDtoBuilder, UserDtoBuilder>()
                .AddTransient<IErrorDtoBuilder, ErrorDtoBuilder>()
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddMediatR(cfg => {
                    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
                });
            return services;
        }
    }
}
