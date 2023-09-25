using BMJ.Authenticator.Domain.Common.Errors.Builders;
using BMJ.Authenticator.Domain.Common.Results.Builders;
using BMJ.Authenticator.Domain.Entities.Users.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace BMJ.Authenticator.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
        => services
        .AddTransient<IErrorBuilder, ErrorBuilder>()
        .AddTransient<IResultBuilder, ResultBuilder>()
        .AddTransient<IResultGenericBuilder, ResultGenericBuilder>()
        .AddTransient<IUserBuilder, UserBuilder>();
}
