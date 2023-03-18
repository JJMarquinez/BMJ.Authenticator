using BMJ.Authenticator.Domain.Common;
using BMJ.Authenticator.Domain.ValueObjects;

namespace BMJ.Authenticator.Domain.UnitTests.Common;

public class ErrorTests
{
    Error _error;
    string _code;
    string _title;
    string _detail;
    int _httpStatusCode;
    public ErrorTests()
    {
        _code = "Identity.InvalidOperation.UserNameOrPasswordNotValid";
        _title = "User name or password aren't valid.";
        _detail = "The user name or password wich were sent are not correct, either the user doesn't exist or password isn't correct.";
        _httpStatusCode = 409;
        _error = Error.New(_code, _title, _detail, _httpStatusCode);
    }

    [Fact]
    public void Should_ThrowArgumentNullException_When_IsAttemptToBeCreatedWithNullCode()
    {
        Assert.Throws<ArgumentNullException>(()
            => {
                Error.New(
                    null, 
                    _title,
                    _detail,
                    _httpStatusCode);
            });
    }

    [Fact]
    public void Should_ThrowArgumentException_When_IsAttemptToBeCreatedWithEmptyCode()
    {
        Assert.Throws<ArgumentException>(()
            => {
                Error.New(
                    string.Empty,
                    _title,
                    _detail,
                    _httpStatusCode);
            });
    }

    [Fact]
    public void Should_ThrowArgumentNullException_When_IsAttemptToBeCreatedWithNullTitle()
    {
        Assert.Throws<ArgumentNullException>(()
            => {
                Error.New(
                    _code,
                    null,
                    _detail,
                    _httpStatusCode);
            });
    }

    [Fact]
    public void Should_ThrowArgumentException_When_IsAttemptToBeCreatedWithEmptyTitle()
    {
        Assert.Throws<ArgumentException>(()
            => {
                Error.New(
                    _code,
                    string.Empty,
                    _detail,
                    _httpStatusCode);
            });
    }

    [Fact]
    public void Should_ThrowArgumentNullException_When_IsAttemptToBeCreatedWithNullDetail()
    {
        Assert.Throws<ArgumentNullException>(()
            => {
                Error.New(
                    _code,
                    _title,
                    null,
                    _httpStatusCode);
            });
    }

    [Fact]
    public void Should_ThrowArgumentException_When_IsAttemptToBeCreatedWithEmptyDetail()
    {
        Assert.Throws<ArgumentException>(()
            => {
                Error.New(
                    _code,
                    _title,
                    string.Empty,
                    _httpStatusCode);
            });
    }

    [Fact]
    public void Should_ThrowArgumentNullException_When_IsAttemptToBeCreatedWithZeroAsAValueOfHttpStatusCode()
    {
        Assert.Throws<ArgumentException>(()
            => {
                Error.New(
                    _code,
                    _title,
                    _detail,
                    0);
            });
    }

    [Fact]
    public void Should_CreateNewError_When_ParametersAreValid()
    {
        Assert.NotNull(Error.New(_code, _title, _detail, _httpStatusCode));
    }

    [Fact]
    public void Should_GetCode_When_ErrorIsCreated()
    {
        Assert.Equal(_code, _error.GetCode());
    }

    [Fact]
    public void Should_GetTitle_When_ErrorIsCreated()
    {
        Assert.Equal(_title, _error.GetTitle());
    }

    [Fact]
    public void Should_GetDetail_When_ErrorIsCreated()
    {
        Assert.Equal(_detail, _error.GetDetail());
    }

    [Fact]
    public void Should_GetHttpStatusCode_When_ErrorIsCreated()
    {
        Assert.Equal(_httpStatusCode, _error.GetHttpStatusCode());
    }

    [Fact]
    public void Should_ConvertToString_When_PerformImplicitOperator()
    {
        string errorCode = _error;
        Assert.Equal(_error.GetCode(), errorCode);
    }

    [Fact]
    public void Should_BeEqual_When_HasTheSameDataWithOther()
    {
        Error none = Error.New("None", "None", "None", 200);
        Assert.True(none.Equals(Error.None));
    }

    [Fact]
    public void Should_NotBeEqual_When_HasDifferentDataWithOther()
    {
        Assert.False(_error.Equals(Error.None));
    }

    [Fact]
    public void Should_NotBeEqual_When_TheOtherIsNull()
    {
        Assert.False(_error.Equals(null));
    }

    [Fact]
    public void Should_NotBeEqual_When_TheOtherIsNotError()
    {
        Assert.False(_error.Equals(new object()));
    }
}
