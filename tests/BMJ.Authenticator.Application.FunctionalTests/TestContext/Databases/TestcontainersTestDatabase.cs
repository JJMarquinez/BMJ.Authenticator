using BMJ.Authenticator.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Respawn;
using System.Data.Common;
using Testcontainers.MsSql;

namespace BMJ.Authenticator.Application.FunctionalTests.TestContext.Databases;

public class TestcontainersTestDatabase : ITestDatabase
{
    private readonly MsSqlContainer _msSqlContainer;
    private DbConnection _dbConnection = null!;
    private Respawner _respawner = null!;

    public TestcontainersTestDatabase()
    {
        _msSqlContainer = new MsSqlBuilder().WithAutoRemove(true).Build();
    }

    public async Task DisposeAsync()
    {
        await _dbConnection.DisposeAsync();
        await _msSqlContainer.DisposeAsync();
    }

    public DbConnection GetDbConnection()
    {
        return _dbConnection;
    }

    public async Task InitialiseAsync()
    {
        await _msSqlContainer.StartAsync();
        string connectionString = _msSqlContainer.GetConnectionString();
        _dbConnection = InitializeDatabaseAsync(connectionString);
        await InitializeRespawnAsync(connectionString);
    }

    private DbConnection InitializeDatabaseAsync(string connectionString)
    {
        var connection = new SqlConnection(connectionString);

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        var context = new ApplicationDbContext(options);

        context.Database.Migrate();

        return connection;
    }

    private async Task InitializeRespawnAsync(string connectionString)
    {
        _respawner = await Respawner.CreateAsync(connectionString, new RespawnerOptions
        {
            TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" }
        });
    }

    public async Task ResetAsync()
    {
        await _respawner.ResetAsync(_msSqlContainer.GetConnectionString());
    }
}
