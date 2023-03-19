using BMJ.Authenticator.Domain.Common;
using BMJ.Authenticator.Domain.Common.Results;

namespace BMJ.Authenticator.Domain.UnitTests.Common.Results;

public class ResultTest
{
    Error _error;
    Result _success;
    Result _failure;
    Result<object?> _successGeneric;
    Result<object?> _failureGeneric;
    public ResultTest()
    {
        _error = Error.New(
            "Identity.InvalidOperation.UserNameOrPasswordNotValid",
            "User name or password aren't valid.",
            "The user name or password wich were sent are not correct, either the user doesn't exist or password isn't correct.",
            409);
        _success = Result.Success();
        _failure = Result.Failure(_error);
        _successGeneric = Result.Success<object?>(new());
        _failureGeneric = Result.Failure<object?>(_error);
    }

    [Fact]
    public void Should_ThrowArgumentNullException_When_IsAttemptToBeCreatedWithNullError()
    {
        Assert.Throws<ArgumentNullException>(() => { Result.Failure(null); });
    }

    [Fact]
    public void Should_ThrowArgumentNullException_When_IsAttemptToCreateGenericResultWithNullError()
    {
        Assert.Throws<ArgumentNullException>(() => { Result.Failure<object>(null); });
    }

    [Fact]
    public void Should_BeCreated_When_IsAttemptToCreateWithError()
    {
        Assert.NotNull(Result.Failure(_error));
    }

    [Fact]
    public void Should_BeCreated_When_IsAttemptToCreateGenericResultWithError()
    {
        Assert.NotNull(Result.Failure<object?>(_error));
    }

    [Fact]
    public void Should_BeCreated_When_IsAttemptToCreateWith0utError()
    {
        Assert.NotNull(Result.Success());
    }

    [Fact]
    public void Should_BeCreated_When_IsAttemptToCreateGenericResultWithoutError()
    {
        Assert.NotNull(Result.Success<object?>(new()));
    }

    [Fact]
    public void Should_GetError_When_ItHasAnError()
    {
        Result result = Result.Failure(_error);
        Assert.True(_error.Equals(result.GetError()));
    }

    [Fact]
    public void Should_GetError_When_GenericResultHasAnError()
    {
        Result<object?> result = Result.Failure<object?>(_error);
        Assert.True(_error.Equals(result.GetError()));
    }

    [Fact]
    public void Should_BeSuccess_When_ItGetsCreatedAsSuccess()
    {
        Assert.True(_success.IsSuccess());
    }

    [Fact]
    public void Should_BeSuccess_When_GenereicResultGetsCreatedAsSuccess()
    {
        Assert.True(_successGeneric.IsSuccess());
    }

    [Fact]
    public void Should_BeUnsuccess_When_ItGetsCreatedAsFailure()
    {
        Assert.False(_failure.IsSuccess());
    }

    [Fact]
    public void Should_BeUnsuccess_When_GenericResultGetsCreatedAsFailure()
    {
        Assert.False(_failureGeneric.IsSuccess());
    }

    [Fact]
    public void Should_BeFailure_When_ItGetsCreatedAsFailure()
    {
        Assert.True(_failure.IsFailure());
    }

    [Fact]
    public void Should_BeFailure_When_GenericResultGetsCreatedAsFailure()
    {
        Assert.True(_failureGeneric.IsFailure());
    }

    [Fact]
    public void Should_NotBeFailure_When_ItGetsCreatedAsSuccess()
    {
        Assert.False(_success.IsFailure());
    }

    [Fact]
    public void Should_NotBeFailure_When_GenericResultGetsCreatedAsSuccess()
    {
        Assert.False(_successGeneric.IsFailure());
    }

    [Fact]
    public void Should_BeCreated_When_PerformImplictOperator()
    {
        Result<Guid> result = Guid.NewGuid();
        Assert.NotNull(result);
    }

    [Fact]
    public void Should_BeCreatedAsASuccessGenericResult_When_PerformImplictOperator()
    {
        Result<Guid> result = Guid.NewGuid();
        Assert.True(result.IsSuccess());
    }

    [Fact]
    public void Should_GetValue_When_GenericResultIsCreatedByPerformingImplictOperator()
    {
        Guid guid = Guid.NewGuid();
        var result = (Result<Guid>)guid;
        Assert.True(guid.Equals(result.GetValue()));
    }
}
