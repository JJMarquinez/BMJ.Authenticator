using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Application.FunctionalTests.TestContext.Databases;
using BMJ.Authenticator.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BMJ.Authenticator.Application.FunctionalTests.TestContext;

public class AuthenticatorTestConext : IDisposable
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

        return await mediator.Send(request).ConfigureAwait(false);
    }

    public async Task ResetState()
    {
        await _database.ResetAsync();
    }

    public async ValueTask<string?> AddAsync(UserDto userDto, string password)
    {
        string? userId = null!;
        using var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var user = ApplicationUser.Builder()
            .WithUserName(userDto.UserName)
            .WithEmail(userDto.Email)
            .WithPhoneNumber(userDto.PhoneNumber)
            .Build();

        var userResult = await userManager.CreateAsync(user, password).ConfigureAwait(false);

        if (userResult.Succeeded)    
        {
            user = await userManager.Users.FirstOrDefaultAsync(user => user.UserName == userDto.UserName).ConfigureAwait(false);
            userId = user?.Id;

            if (userDto.Roles.Any())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                foreach (var role in userDto.Roles)
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(role)).ConfigureAwait(false);
                    if (roleResult.Succeeded)
                        await userManager.AddToRolesAsync(user!, userDto.Roles).ConfigureAwait(false);
                }
            }
        }

        return userId;
    }

    public async void Dispose()
    {
        await _database.DisposeAsync();
        await _factory.DisposeAsync().ConfigureAwait(false);
    }
}
