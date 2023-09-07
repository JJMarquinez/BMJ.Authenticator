using BMJ.Authenticator.Api.FunctionalTests.TestContext.Cache;
using BMJ.Authenticator.Api.FunctionalTests.TestContext.Databases;
using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BMJ.Authenticator.Api.FunctionalTests.TestContext;

public class AuthenticatorTestConext : IDisposable
{
    private static ITestDatabase _database = null!;
    private static ITestCache _cache = null!;
    private static AuthenticatorWebApplicationFactory _factory = null!;
    private static IServiceScopeFactory _scopeFactory = null!;

    public AuthenticatorTestConext()
    {
        _database = new MsSqlContainerTestDatabase();
        _database.InitialiseAsync().Wait();

        _cache = new RedisContainerTestCache();
        _cache.InitialiseAsync().Wait();

        _factory = new AuthenticatorWebApplicationFactory(_database.GetDbConnection(), _cache.GetConnectionString());

        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
    }

    public async Task<HttpResponseMessage> GetAsync(string? requestUri, IBaseRequest? request)
    {
        var client = _factory.CreateClient();
        var content = request is not null ? JsonSerializer.Serialize(request, request.GetType()) : null;
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUri);
        if (content is not null)
        {
            httpRequest.Content = new StringContent(content, Encoding.UTF8);
            httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
        return await client.SendAsync(httpRequest);
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
        await _cache.DisposeAsync();
        await _factory.DisposeAsync().ConfigureAwait(false);
    }
}
