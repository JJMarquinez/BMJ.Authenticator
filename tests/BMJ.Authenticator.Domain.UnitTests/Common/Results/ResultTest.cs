using BMJ.Authenticator.Domain.Common;
using BMJ.Authenticator.Domain.Common.Results;

namespace BMJ.Authenticator.Domain.UnitTests.Common.Results;

public class ResultTest
{
    Error _error;
    public ResultTest()
    {
        _error = Error.New(
            "Identity.InvalidOperation.UserNameOrPasswordNotValid",
            "User name or password aren't valid.",
            "The user name or password wich were sent are not correct, either the user doesn't exist or password isn't correct.",
            409);
    }

    [Fact]
    public void Should_ThrowArgumentNullException_When_IsAttemptToBeCreatedWithNullError()
    {
        Assert.Throws<ArgumentNullException>(() => { Result.Failure(null); });
    }

    [Fact]
    public void Should_BeCreated_When_IsAttemptToCreateWithError()
    {
        Assert.NotNull(Result.Failure(_error));
    }
}
