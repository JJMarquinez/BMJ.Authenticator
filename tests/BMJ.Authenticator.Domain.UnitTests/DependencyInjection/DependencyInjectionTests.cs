using BMJ.Authenticator.Domain.Common.Errors.Builders;
using BMJ.Authenticator.Domain.Common.Results.FactoryMethods;
using BMJ.Authenticator.Domain.Entities.Users.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace BMJ.Authenticator.Domain.UnitTests.DependencyInjection;

public class DependencyInjectionTests
{
    private readonly IServiceCollection _serviceCollection;

    public DependencyInjectionTests()
    {
        _serviceCollection = new ServiceCollection();
        _serviceCollection.AddDomainServices();
    }

    [Fact]
    public void ShouldAddDomainServices()
    {
        var serviceProvider = _serviceCollection.BuildServiceProvider();

        Assert.IsType<ErrorBuilder>(serviceProvider.GetService<IErrorBuilder>());
        Assert.IsType<ResultFactory>(serviceProvider.GetService<IResultFactory>());
        Assert.IsType<ResultGenericFactory>(serviceProvider.GetService<IResultGenericFactory>());
        Assert.IsType<ResultCreator>(serviceProvider.GetService<IResultCreator>());
        Assert.IsType<UserBuilder>(serviceProvider.GetService<IUserBuilder>());
    }
}
