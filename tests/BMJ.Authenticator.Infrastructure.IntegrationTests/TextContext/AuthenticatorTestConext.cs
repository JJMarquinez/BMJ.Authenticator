using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Infrastructure.Identity;
using BMJ.Authenticator.Infrastructure.IntegrationTests.TextContext.Databases;
using BMJ.Authenticator.Infrastructure.Loggers;
using BMJ.Authenticator.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BMJ.Authenticator.Infrastructure.IntegrationTests.TextContext;

public class AuthenticatorTestConext : IDisposable
{
    private static ITestDatabase _database = null!;
    private static IServiceScopeFactory _scopeFactory = null!;

    public AuthenticatorTestConext()
    {
        _database = new MsSqlContainerTestDatabase();
        _database.InitialiseAsync().Wait();

        _scopeFactory = ConfigureTestServices().BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
    }

    private IServiceCollection ConfigureTestServices()
    {
        IServiceCollection services = new ServiceCollection();

        services
            .AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(_database.GetDbConnection()))
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services
            .AddTransient<IIdentityService, IdentityService>()
            .AddTransient<IAuthLogger, AuthLogger>();

        return services;
    }

    public IIdentityService GetIdentityService() 
        => _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IIdentityService>();

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
    }
}
