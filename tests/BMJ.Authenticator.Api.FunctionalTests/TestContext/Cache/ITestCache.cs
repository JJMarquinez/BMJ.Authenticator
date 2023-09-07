using System.Data.Common;

namespace BMJ.Authenticator.Api.FunctionalTests.TestContext.Cache;

public interface ITestCache
{
    Task InitialiseAsync();

    string GetConnectionString();

    ValueTask DisposeAsync();
}
