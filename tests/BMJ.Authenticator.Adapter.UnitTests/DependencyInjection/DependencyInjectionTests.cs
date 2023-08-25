using BMJ.Authenticator.Adapter.Authentication;
using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Adapter.Identity;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BMJ.Authenticator.Adapter.UnitTests.DependencyInjection;

public class DependencyInjectionTests
{
    private readonly IServiceCollection _serviceCollection;
    public DependencyInjectionTests()
    {
        _serviceCollection = new ServiceCollection();
        _serviceCollection.AddTransient<IIdentityService, IdentityServiceTest>();
        _serviceCollection.AddTransient<IAuthLogger, AuthLoggerTest>();
        _serviceCollection.Configure<JwtOptions>(new ConfigurationManager().GetSection(nameof(JwtOptions)));

        _serviceCollection.AddAdapterServices();
    }

    [Fact]
    public void ShouldAddAutoMapperServices()
    {
        var serviceProvider = _serviceCollection.BuildServiceProvider();

        Assert.IsType<IdentityAdapter>(serviceProvider.GetService<IIdentityAdapter>());
        Assert.IsType<JwtProvider>(serviceProvider.GetService<IJwtProvider>());
    }
}