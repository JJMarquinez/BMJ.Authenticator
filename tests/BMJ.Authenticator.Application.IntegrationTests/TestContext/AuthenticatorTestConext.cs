using BMJ.Authenticator.Infrastructure.Identity;
using BMJ.Authenticator.ToolKit.Database.Abstractions;
using BMJ.Authenticator.ToolKit.Database.Testcontainters;
using BMJ.Authenticator.ToolKit.Identity.UserOperators;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BMJ.Authenticator.Application.FunctionalTests.TestContext;

public class AuthenticatorTestConext : IDisposable
{
    private ITestDatabase _database = null!;
    private AuthenticatorWebApplicationFactory _factory = null!;
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

        _factory = new AuthenticatorWebApplicationFactory(_database.GetDbConnection());

        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();

        var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        _userOperator = new UserOperator(userManager, roleManager);
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request).ConfigureAwait(false);
    }

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
        await _factory.DisposeAsync().ConfigureAwait(false);
        _userOperator.Dispose();
    }
}
