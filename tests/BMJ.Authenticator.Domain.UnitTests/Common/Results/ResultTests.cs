using BMJ.Authenticator.Domain.Common;
using BMJ.Authenticator.Domain.Common.Results;

namespace BMJ.Authenticator.Domain.UnitTests.Common.Results;

public class ResultTests
{
    Error _error;

    public ResultTests()
    {
        _error = Error.New(
            "Identity.InvalidOperation.UserNameOrPasswordNotValid",
            "User name or password aren't valid.",
            "The user name or password wich were sent are not correct, either the user doesn't exist or password isn't correct.",
            409);
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullAsError()
    {
        Assert.Throws<ArgumentNullException>(() => { Result.Failure(null!); });
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullAsErrorToGenericResult()
    {
        Assert.Throws<ArgumentNullException>(() => { Result.Failure<object>(null!); });
    }

    [Fact]
    public void ShouldBeCreatedAResultGivenACreatedError()
    {
        Assert.NotNull(Result.Failure(_error));
    }

    [Fact]
    public void ShouldBeCreatedAGenericReultGivenACreatedError()
    {
        Assert.NotNull(Result.Failure<object?>(_error));
    }

    [Fact]
    public void ShouldBeCreatedASuccessResult()
    {
        Assert.NotNull(Result.Success());
    }

    [Fact]
    public void ShouldBeCreatedAGenericSuccessResult()
    {
        Assert.NotNull(Result.Success<object?>(new()));
    }

    [Fact]
    public void ShouldGetErrorGivenAError()
    {
        Result result = Result.Failure(_error);
        Assert.True(_error.Equals(result.GetError()));
    }

    [Fact]
    public void ShouldGetErrorGivenAErrorToGenericResult()
    {
        Result<object?> result = Result.Failure<object?>(_error);
        Assert.True(_error.Equals(result.GetError()));
    }

    [Fact]
    public void ShouldBeSuccessResult()
    {
        Assert.True(Result.Success().IsSuccess());
    }

    [Fact]
    public void ShouldBeGenericSuccessResult()
    {
        Assert.True(Result.Success<object?>(new()).IsSuccess());
    }

    [Fact]
    public void ShouldBeUnsuccessResult()
    {
        Assert.False(Result.Failure(_error).IsSuccess());
    }

    [Fact]
    public void ShouldBeGenericUnsuccessResult()
    {
        Assert.False(Result.Failure<object?>(_error).IsSuccess());
    }

    [Fact]
    public void ShouldBeFailureResult()
    {
        Assert.True(Result.Failure(_error).IsFailure());
    }

    [Fact]
    public void ShouldBeGenericFailureResult()
    {
        Assert.True(Result.Failure<object?>(_error).IsFailure());
    }

    [Fact]
    public void ShouldNotBeFailureResultGivenSuccessResult()
    {
        Assert.False(Result.Success().IsFailure());
    }

    [Fact]
    public void ShouldNotBeGenericFailureResultGivenGenericSuccessResult()
    {
        Assert.False(Result.Success<object?>(new()).IsFailure());
    }

    [Fact]
    public void ShouldBeCreated()
    {
        Result<Guid> result = Guid.NewGuid();
        Assert.NotNull(result);
    }

    [Fact]
    public void ShouldBeCreatedAsASuccessGenericResult()
    {
        Result<Guid> result = Guid.NewGuid();
        Assert.True(result.IsSuccess());
    }

    [Fact]
    public void ShouldGetValueGivenGenericSuccessResultCreatedByPerformingImplicitOperator()
    {
        Guid guid = Guid.NewGuid();
        var result = (Result<Guid>)guid;
        Assert.True(guid.Equals(result.GetValue()));
    }
}
