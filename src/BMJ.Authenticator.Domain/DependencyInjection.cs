using BMJ.Authenticator.Domain.Common.Errors.Builders;
using BMJ.Authenticator.Domain.Common.Results.FactoryMethods;
using Microsoft.Extensions.DependencyInjection;

namespace BMJ.Authenticator.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
        => services
        .AddTransient<IErrorBuilder, ErrorBuilder>()
        .AddTransient<IResultFactory, ResultFactory>()
        .AddTransient<IResultGenericFactory, ResultGenericFactory>()
        .AddTransient<IResultCreator, ResultCreator>()
        //.AddTransient(typeof(IResultBuilder<>), typeof(ResultBuilder<>))
        //.AddTransient<IProviderBuilder, ProviderBuilder>()
        ;
}
