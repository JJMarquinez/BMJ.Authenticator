using BMJ.Authenticator.Domain.Common.Errors;
using BMJ.Authenticator.Domain.Common.Errors.Builders;
using BMJ.Authenticator.Domain.Common.Results;
using BMJ.Authenticator.Domain.Common.Results.Builders;

namespace BMJ.Authenticator.Domain.UnitTests.Common.Results;

public class ResultTests
{
    private readonly IResultBuilder _resultBuilder;
    private readonly IResultGenericBuilder _resultGenericBuilder;
    private readonly Error _error;

    public ResultTests()
    {
        _resultBuilder = new ResultBuilder();
        _resultGenericBuilder = new ResultGenericBuilder();
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
        Assert.Throws<ArgumentNullException>(() => { _resultBuilder.WithError(null!).Build(); });
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenNoneErrorAttemptingToCreateFailureResult()
    {
        Assert.Throws<ArgumentException>(() => { _resultBuilder.WithError(Error.None).Build(); });
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullAsErrorToGenericResult()
    {
        Assert.Throws<ArgumentNullException>(() => { _resultGenericBuilder.BuildFailure<object>(null!); });
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenNoneErrorAttemptingToCreateGenericFailureResult()
    {
        Assert.Throws<ArgumentException>(() => { _resultGenericBuilder.BuildFailure<object>(Error.None); });
    }

    [Fact]
    public void ShouldBeCreatedAResultGivenACreatedError()
    {
        Assert.NotNull(_resultBuilder.WithError(_error).Build());
    }

    [Fact]
    public void ShouldBeCreatedAGenericReultGivenACreatedError()
    {
        Assert.NotNull(_resultGenericBuilder.BuildFailure<object?>(_error));
    }

    [Fact]
    public void ShouldBeCreatedASuccessResult()
    {
        Assert.NotNull(_resultBuilder.BuildSuccess());
    }

    [Fact]
    public void ShouldBeCreatedAGenericSuccessResult()
    {
        Assert.NotNull(_resultGenericBuilder.BuildSuccess<object?>(new()));
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenNoValueCreatingGenericSuccessResult()
    {
        Assert.Throws<ArgumentException>(() => { _resultGenericBuilder.BuildSuccess<object?>(default); });
    }

    [Fact]
    public void ShouldGetErrorGivenAError()
    {
        Result result = _resultBuilder.WithError(_error).Build();
        Assert.True(_error.Equals(result.Error));
    }

    [Fact]
    public void ShouldGetErrorGivenAErrorToGenericResult()
    {
        Result<object?> result = _resultGenericBuilder.BuildFailure<object?>(_error);
        Assert.True(_error.Equals(result.Error));
    }

    [Fact]
    public void ShouldBeSuccessResult()
    {
        Assert.True(_resultBuilder.BuildSuccess().IsSuccess());
    }

    [Fact]
    public void ShouldBeGenericSuccessResult()
    {
        Assert.True(_resultGenericBuilder.BuildSuccess<object?>(new()).IsSuccess());
    }

    [Fact]
    public void ShouldBeUnsuccessResult()
    {
        Assert.False(_resultBuilder.WithError(_error).Build().IsSuccess());
    }

    [Fact]
    public void ShouldBeGenericUnsuccessResult()
    {
        Assert.False(_resultGenericBuilder.BuildFailure<object?>(_error).IsSuccess());
    }

    [Fact]
    public void ShouldBeFailureResult()
    {
        Assert.True(_resultBuilder.WithError(_error).Build().IsFailure());
    }

    [Fact]
    public void ShouldBeGenericFailureResult()
    {
        Assert.True(_resultGenericBuilder.BuildFailure<object?>(_error).IsFailure());
    }

    [Fact]
    public void ShouldNotBeFailureResultGivenSuccessResult()
    {
        Assert.False(_resultBuilder.BuildSuccess().IsFailure());
    }

    [Fact]
    public void ShouldNotBeGenericFailureResultGivenGenericSuccessResult()
    {
        Assert.False(_resultGenericBuilder.BuildSuccess<object?>(new()).IsFailure());
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
        Result<string> result = _resultGenericBuilder.BuildSuccess(guid);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void ShouldResultValueBeNullGivenFailureResult()
    {
        string guid = Guid.NewGuid().ToString();
        Result<string?> result = _resultGenericBuilder.BuildFailure<string?>(_error);
        Assert.Null(result.Value);
    }
}
