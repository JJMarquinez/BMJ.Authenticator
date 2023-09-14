using System.Data.Common;

namespace BMJ.Authenticator.Adapter.IntegrationTests.TextContext.Databases;

public interface ITestDatabase
{
    Task InitialiseAsync();

    DbConnection GetDbConnection();

    ValueTask ResetAsync();

    ValueTask DisposeAsync();
}

