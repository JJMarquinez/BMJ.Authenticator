using BMJ.Authenticator.Domain.Common.Errors;
using BMJ.Authenticator.Domain.Common.Errors.Builders;
using BMJ.Authenticator.Domain.Common.Results;
using BMJ.Authenticator.Domain.Common.Results.Factories;

namespace BMJ.Authenticator.Domain.UnitTests.Common.Results;

public class ResultTests
{
    private readonly IResultCreator _resultCreator;
    private readonly Error _error;

    public ResultTests()
    {
        _resultCreator = new ResultCreator(new ResultFactory(), new ResultGenericFactory());
        _error = new ErrorBuilder()
            .WithCode("Identity.InvalidOperation.UserNameOrPasswordNotValid")
            .WithTitle("User name or password aren't valid.")
            .WithDetail("The user name or password wich were sent are not correct, either the user doesn't exist or password isn't correct.")
            .WithHttpStatusCode(409)
            .Build();
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullAsError()
    {
        Assert.Throws<ArgumentNullException>(() => { _resultCreator.CreateFailureResult(null!); });
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenNoneErrorAttemptingToCreateFailureResult()
    {
        Assert.Throws<ArgumentException>(() => { _resultCreator.CreateFailureResult(Error.None); });
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullAsErrorToGenericResult()
    {
        Assert.Throws<ArgumentNullException>(() => { _resultCreator.CreateFailureResult<object>(null!); });
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenNoneErrorAttemptingToCreateGenericFailureResult()
    {
        Assert.Throws<ArgumentException>(() => { _resultCreator.CreateFailureResult<object>(Error.None); });
    }

    [Fact]
    public void ShouldBeCreatedAResultGivenACreatedError()
    {
        Assert.NotNull(_resultCreator.CreateFailureResult(_error));
    }

    [Fact]
    public void ShouldBeCreatedAGenericReultGivenACreatedError()
    {
        Assert.NotNull(_resultCreator.CreateFailureResult<object?>(_error));
    }

    [Fact]
    public void ShouldBeCreatedASuccessResult()
    {
        Assert.NotNull(_resultCreator.CreateSuccessResult());
    }

    [Fact]
    public void ShouldBeCreatedAGenericSuccessResult()
    {
        Assert.NotNull(_resultCreator.CreateSuccessResult<object?>(new()));
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenNoValueCreatingGenericSuccessResult()
    {
        Assert.Throws<ArgumentException>(() => { _resultCreator.CreateSuccessResult<object?>(default); });
    }

    [Fact]
    public void ShouldGetErrorGivenAError()
    {
        Result result = _resultCreator.CreateFailureResult(_error);
        Assert.True(_error.Equals(result.Error));
    }

    [Fact]
    public void ShouldGetErrorGivenAErrorToGenericResult()
    {
        Result<object?> result = _resultCreator.CreateFailureResult<object?>(_error);
        Assert.True(_error.Equals(result.Error));
    }

    [Fact]
    public void ShouldBeSuccessResult()
    {
        Assert.True(_resultCreator.CreateSuccessResult().IsSuccess());
    }

    [Fact]
    public void ShouldBeGenericSuccessResult()
    {
        Assert.True(_resultCreator.CreateSuccessResult<object?>(new()).IsSuccess());
    }

    [Fact]
    public void ShouldBeUnsuccessResult()
    {
        Assert.False(_resultCreator.CreateFailureResult(_error).IsSuccess());
    }

    [Fact]
    public void ShouldBeGenericUnsuccessResult()
    {
        Assert.False(_resultCreator.CreateFailureResult<object?>(_error).IsSuccess());
    }

    [Fact]
    public void ShouldBeFailureResult()
    {
        Assert.True(_resultCreator.CreateFailureResult(_error).IsFailure());
    }

    [Fact]
    public void ShouldBeGenericFailureResult()
    {
        Assert.True(_resultCreator.CreateFailureResult<object?>(_error).IsFailure());
    }

    [Fact]
    public void ShouldNotBeFailureResultGivenSuccessResult()
    {
        Assert.False(_resultCreator.CreateSuccessResult().IsFailure());
    }

    [Fact]
    public void ShouldNotBeGenericFailureResultGivenGenericSuccessResult()
    {
        Assert.False(_resultCreator.CreateSuccessResult<object?>(new()).IsFailure());
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
        Assert.True(guid.Equals(result.Value));
    }

    [Fact]
    public void ShouldResultValueNotToBeNullGivenSuccessResult()
    {
        string guid = Guid.NewGuid().ToString();
        Result<string> result = _resultCreator.CreateSuccessResult(guid);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void ShouldResultValueBeNullGivenFailureResult()
    {
        string guid = Guid.NewGuid().ToString();
        Result<string?> result = _resultCreator.CreateFailureResult<string?>(_error);
        Assert.Null(result.Value);
    }
}
