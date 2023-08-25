using BMJ.Authenticator.Common.UnitTests.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BMJ.Authenticator.Common.UnitTests.DependencyInjection;

public sealed class ServiceCollectionVerifier
{
    private readonly Mock<IServiceCollection> serviceCollectionMock;

    public ServiceCollectionVerifier()
    {
        serviceCollectionMock = new();
    }

    public IServiceCollection ServiceCollection => serviceCollectionMock.Object;

    public void ContainsSingletonService<TService, TInstance>()
    {
        IsRegistered<TService, TInstance>(ServiceLifetime.Singleton);
    }

    public void ContainsTransientService<TService, TInstance>()
    {
        IsRegistered<TService, TInstance>(ServiceLifetime.Transient);
    }

    public void ContainsScopedService<TService, TInstance>()
    {
        IsRegistered<TService, TInstance>(ServiceLifetime.Scoped);
    }

    private void IsRegistered<TService, TInstance>(ServiceLifetime lifetime)
    {
        serviceCollectionMock
            .Verify(serviceCollection => serviceCollection.Add(
                It.Is<ServiceDescriptor>(serviceDescriptor => serviceDescriptor.Is<TService, TInstance>(lifetime))));

    }
}
