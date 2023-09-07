using System.Data.Common;

namespace BMJ.Authenticator.Api.FunctionalTests.TestContext.Databases;

public interface ITestDatabase
{
    Task InitialiseAsync();

    DbConnection GetDbConnection();

    ValueTask ResetAsync();

    ValueTask DisposeAsync();
}
