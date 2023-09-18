using BMJ.Authenticator.Application.Common.Models.Results.FactoryMethods;
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
                .AddTransient<IResultDtoFactory, ResultDtoFactory>()
                .AddTransient<IResultDtoGenericFactory, ResultDtoGenericFactory>()
                .AddTransient<IResultDtoCreator, ResultDtoCreator>()
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
