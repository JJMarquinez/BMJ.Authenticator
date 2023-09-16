
using BMJ.Authenticator.Api.Caching;
using BMJ.Authenticator.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;
using System.Data.Common;

namespace BMJ.Authenticator.Api.FunctionalTests.TestContext;

public class AuthenticatorWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly DbConnection _connection;
    private readonly string _redisConnectionString;

    public AuthenticatorWebApplicationFactory(DbConnection connection, string redisConfiguration)
    {
        _connection = connection;
        _redisConnectionString = redisConfiguration;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, conf) =>
        {
            conf.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>()
            .AddDbContextPool<ApplicationDbContext>((sp, options) =>
            {
                options.UseSqlServer(_connection);
            })
            .RemoveAll<IConnectionMultiplexer>()
            .AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(_redisConnectionString))
            .AddRedisOutputCache(options =>
            {
                options.AddAuthenticatorBaseCachePolicy();

                options.AddPolicy(nameof(ByIdCachePolicy), builder =>
                {
                    builder.NoCache();
                });

                options.AddPolicy(nameof(TokenCachePolicy), builder =>
                {
                    builder.NoCache();
                });
            });
        });
    }
}
