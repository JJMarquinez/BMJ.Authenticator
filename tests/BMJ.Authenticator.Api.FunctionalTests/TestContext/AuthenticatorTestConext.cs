using BMJ.Authenticator.Api.FunctionalTests.TestContext.Cache;
using BMJ.Authenticator.Api.FunctionalTests.TestContext.Databases;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Infrastructure.Identity;
using BMJ.Authenticator.Tool.Identity.UserOperators;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
    private static IUserOperator _userOperator = null!;

    public AuthenticatorTestConext()
    {
        Initialize();
    }

    private void Initialize()
    {
        _database = new MsSqlContainerTestDatabase();
        _database.InitialiseAsync().Wait();

        _cache = new RedisContainerTestCache();
        _cache.InitialiseAsync().Wait();

        _factory = new AuthenticatorWebApplicationFactory(_database.GetDbConnection(), _cache.GetConnectionString());

        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();

        var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        _userOperator = new UserOperator(userManager, roleManager);
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
        var user = new UserDto
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Joe",
            Email = "joe@authenticator.com",
            PhoneNumber = "111-444-777",
            Roles = new[] { "Guest" }
        };
        var jwtProvider = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IJwtProvider>();
        return await jwtProvider.GenerateAsync(user);
    }

    public async Task ResetState()
    {
        await _database.ResetAsync();
    }

    public async ValueTask<string?> AddAsync(ApplicationUser applicationUser, string password, string[] roles)
        => await _userOperator.AddAsync(applicationUser, password, roles);

    public async ValueTask<ApplicationUser?> FindAsync(string applicationUserId)
        => await _userOperator.FindAsync(applicationUserId);

    public async void Dispose()
    {
        await _database.DisposeAsync();
        await _cache.DisposeAsync();
        await _factory.DisposeAsync().ConfigureAwait(false);
        _userOperator.Dispose();
    }
}
