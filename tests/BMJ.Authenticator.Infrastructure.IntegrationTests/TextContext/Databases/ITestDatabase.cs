using System.Data.Common;

namespace BMJ.Authenticator.Infrastructure.IntegrationTests.TextContext.Databases;

public interface ITestDatabase
{
    Task InitialiseAsync();

    DbConnection GetDbConnection();

    ValueTask ResetAsync();

    ValueTask DisposeAsync();
}
