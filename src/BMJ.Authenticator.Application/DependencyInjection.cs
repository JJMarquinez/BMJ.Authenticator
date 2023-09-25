using BMJ.Authenticator.Application.Common.Models.Errors.Builders;
using BMJ.Authenticator.Application.Common.Models.Results.Builders;
using BMJ.Authenticator.Application.Common.Models.Users.Builders;
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
                .AddTransient<IResultDtoBuilder, ResultDtoBuilder>()
                .AddTransient<IResultDtoGenericBuilder, ResultDtoGenericBuilder>()
                .AddTransient<IUserDtoBuilder, UserDtoBuilder>()
                .AddTransient<IErrorDtoBuilder, ErrorDtoBuilder>()
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
