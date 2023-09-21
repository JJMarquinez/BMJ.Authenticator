using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Errors.Builders;
using BMJ.Authenticator.Application.Common.Models.Results.FactoryMethods;
using BMJ.Authenticator.Application.Common.Models.Users.Builders;
using BMJ.Authenticator.Infrastructure.Identity;
using BMJ.Authenticator.Infrastructure.Loggers;
using BMJ.Authenticator.Infrastructure.Persistence;
using BMJ.Authenticator.ToolKit.Database.Abstractions;
using BMJ.Authenticator.ToolKit.Database.Testcontainters;
using BMJ.Authenticator.ToolKit.Identity.UserOperators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BMJ.Authenticator.Infrastructure.IntegrationTests.TextContext;

public class AuthenticatorTestConext : IDisposable
{
    private ITestDatabase _database = null!;
    private IServiceScopeFactory _scopeFactory = null!;
    private IUserOperator _userOperator = null!;

    public AuthenticatorTestConext()
    {
        Initialize();
    }

    private void Initialize()
    {
        _database = new MsSqlContainerTestDatabase();
        _database.InitialiseAsync().Wait();

        _scopeFactory = ConfigureTestServices().BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();

        var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        _userOperator = new UserOperator(userManager, roleManager);
    }

    private IServiceCollection ConfigureTestServices()
    {
        IServiceCollection services = new ServiceCollection();

        services
            .AddTransient<IResultDtoFactory, ResultDtoFactory>()
            .AddTransient<IResultDtoGenericFactory, ResultDtoGenericFactory>()
            .AddTransient<IResultDtoCreator, ResultDtoCreator>()
            .AddTransient<IUserDtoBuilder, UserDtoBuilder>()
            .AddTransient<IErrorDtoBuilder, ErrorDtoBuilder>()
            .AddTransient<IIdentityService, IdentityService>()
            .AddTransient<IAuthLogger, AuthLogger>()
            .AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(_database.GetDbConnection()))
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        return services;
    }

    public IIdentityService GetIdentityService() 
        => _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IIdentityService>();

    public async Task ResetState()
    {
        await _database.ResetAsync();
    }

    public async ValueTask<string?> AddAsync(ApplicationUser applicationUser, string password, string[] roles)
        => await _userOperator.AddAsync(applicationUser, password, roles);

    public async ValueTask<ApplicationUser?> FindAsync(string applicationUserId)
    {
        var userManager = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        return await userManager.Users.FirstOrDefaultAsync(user => user.Id == applicationUserId);
    }

    public async void Dispose()
    {
        await _database.DisposeAsync();
        _userOperator.Dispose();
    }
}
