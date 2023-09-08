
using BMJ.Authenticator.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Data.Common;

namespace BMJ.Authenticator.Application.FunctionalTests.TestContext;

public class AuthenticatorWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly DbConnection _connection;

    public AuthenticatorWebApplicationFactory(DbConnection connection)
    {
        _connection = connection;
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
            });
        });
    }
}
