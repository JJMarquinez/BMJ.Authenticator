using System.Data.Common;

namespace BMJ.Authenticator.ToolKit.Database.Abstractions;

public interface ITestDatabase
{
    Task InitialiseAsync();

    DbConnection GetDbConnection();

    ValueTask ResetAsync();

    ValueTask DisposeAsync();
}
