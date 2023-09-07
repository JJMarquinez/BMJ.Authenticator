using BMJ.Authenticator.Api.FunctionalTests.Controllers;
using BMJ.Authenticator.Api.FunctionalTests.TestContext.Cache;
using BMJ.Authenticator.Api.FunctionalTests.TestContext.Databases;
using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Application.UseCases.Users.Queries.LoginUser;
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
        HttpRequestMessage httpRequest = GetHttpRequest(requestUri, request);

        return await client.SendAsync(httpRequest).ConfigureAwait(false);
    }
    
    public async Task<HttpResponseMessage> GetAsync(string? requestUri, IBaseRequest? request, string token)
    {
        var client = _factory.CreateClient();
        HttpRequestMessage httpRequest = GetHttpRequest(requestUri, request, token);

        return await client.SendAsync(httpRequest).ConfigureAwait(false);
    }

    private HttpRequestMessage GetHttpRequest(string? requestUri, IBaseRequest? request, string? token = null)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUri);
        var content = request is not null ? JsonSerializer.Serialize(request, request.GetType()) : null;
        
        if (content is not null)
        {
            httpRequest.Content = new StringContent(content, Encoding.UTF8);
            httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        if(token is not null) 
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return httpRequest;
    }

    public async ValueTask<string> GetTokenAsync()
    {
        var userDto = new UserDto
        {
            UserName = "Megan",
            Email = "megan@authenticator.com",
            PhoneNumber = "111-444-777",
            Roles = new[] { "Guest" }
        };
        await AddAsync(userDto, "A@9&53ro1XG-");
        var request = new LoginUserQuery
        {
            UserName = "Megan",
            Password = "A@9&53ro1XG-"
        };

        var response = await GetAsync(AuthenticatorApi.GetTokenAsync(), request).ConfigureAwait(false);
        return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
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
