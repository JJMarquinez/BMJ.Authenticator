using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Application.FunctionalTests.TestContext.Databases;
using BMJ.Authenticator.Infrastructure.Identity;
using BMJ.Authenticator.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BMJ.Authenticator.Application.FunctionalTests.TestContext;

public class AuthenticatorTestConext
{
    private static ITestDatabase _database = null!;
    private static AuthenticatorWebApplicationFactory _factory = null!;
    private static IServiceScopeFactory _scopeFactory = null!;

    public AuthenticatorTestConext()
    {
        _database = new TestcontainersTestDatabase();
        _database.InitialiseAsync().Wait();

        _factory = new AuthenticatorWebApplicationFactory(_database.GetDbConnection());

        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public async Task SendAsync(IBaseRequest request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        await mediator.Send(request);
    }

    public async Task ResetState()
    {
        await _database.ResetAsync();
    }

    public async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
    where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public async Task AddAsync(UserDto userDto, string password)
    {
        using var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var user = ApplicationUser.Builder()
            .WithUserName(userDto.UserName)
            .WithEmail(userDto.Email)
            .WithPhoneNumber(userDto.PhoneNumber)
            .Build();

        var result = await userManager.CreateAsync(user, password);

        if (userDto.Roles.Any())    
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in userDto.Roles)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            await userManager.AddToRolesAsync(user, userDto.Roles);
        }
    }

    public async Task DisposeAsync()
    {
        await _database.DisposeAsync();
        await _factory.DisposeAsync();
    }
}
