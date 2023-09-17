using BMJ.Authenticator.Domain.Common.Errors.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace BMJ.Authenticator.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
        => services
        .AddTransient<IErrorBuilder, ErrorBuilder>()
        //.AddTransient<IResultBuilder, ResultBuilder>()
        //.AddTransient(typeof(IResultBuilder<>), typeof(ResultBuilder<>))
        //.AddTransient<IProviderBuilder, ProviderBuilder>()
        ;
}
