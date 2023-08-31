using BMJ.Authenticator.Application.FunctionalTests.TestContext.Databases;
using BMJ.Authenticator.Infrastructure.Persistence;
using MediatR;
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

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public static async Task SendAsync(IBaseRequest request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        await mediator.Send(request);
    }

    public static async Task ResetState()
    {
        await _database.ResetAsync();
    }

    public static async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
    where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _database.DisposeAsync();
        await _factory.DisposeAsync();
    }
}
