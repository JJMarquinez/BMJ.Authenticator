using BMJ.Authenticator.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Respawn;
using System.Data.Common;
using Testcontainers.MsSql;

namespace BMJ.Authenticator.Adapter.IntegrationTests.TextContext.Databases;

public class MsSqlContainerTestDatabase : ITestDatabase
{
    private readonly MsSqlContainer _msSqlContainer;
    private DbConnection _dbConnection = null!;
    private Respawner _respawner = null!;

    public MsSqlContainerTestDatabase()
    {
        _msSqlContainer = new MsSqlBuilder().WithAutoRemove(true).Build();
    }

    public async ValueTask DisposeAsync()
    {
        await _dbConnection.DisposeAsync().ConfigureAwait(false);
        await _msSqlContainer.DisposeAsync().ConfigureAwait(false);
    }

    public DbConnection GetDbConnection()
    {
        return _dbConnection;
    }

    public async Task InitialiseAsync()
    {
        await _msSqlContainer.StartAsync().ConfigureAwait(false);
        string connectionString = string.Format("{0};MultipleActiveResultSets=True", _msSqlContainer.GetConnectionString());
        _dbConnection = await InitializeDatabaseAsync(connectionString);
        await InitializeRespawnAsync(connectionString);
    }

    private async ValueTask<DbConnection> InitializeDatabaseAsync(string connectionString)
    {
        var connection = new SqlConnection(connectionString);

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        var context = new ApplicationDbContext(options);

        await context.Database.MigrateAsync().ConfigureAwait(false);

        return connection;
    }

    private async ValueTask InitializeRespawnAsync(string connectionString)
    {
        _respawner = await Respawner.CreateAsync(connectionString, new RespawnerOptions
        {
            TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" }
        }).ConfigureAwait(false);
    }

    public async ValueTask ResetAsync()
    {
        await _respawner.ResetAsync(_msSqlContainer.GetConnectionString()).ConfigureAwait(false);
    }
}