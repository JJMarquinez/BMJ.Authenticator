using System.Data.Common;

namespace BMJ.Authenticator.Application.FunctionalTests.TestContext.Databases;

public interface ITestDatabase
{
    Task InitialiseAsync();

    DbConnection GetDbConnection();

    Task ResetAsync();

    Task DisposeAsync();
}
