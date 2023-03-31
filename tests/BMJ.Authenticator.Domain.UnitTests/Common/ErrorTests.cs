using BMJ.Authenticator.Domain.Common.Errors;

namespace BMJ.Authenticator.Domain.UnitTests.Common;

public class ErrorTests
{
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
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullCode()
    {
        Assert.Throws<ArgumentNullException>(()
            => {
                Error.New(
                    null!, 
                    _title,
                    _detail,
                    _httpStatusCode);
            });
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenEmptyStringAsCode()
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
    public void ShouldThrowArgumentNullExceptionGivenNullTitle()
    {
        Assert.Throws<ArgumentNullException>(()
            => {
                Error.New(
                    _code,
                    null!,
                    _detail,
                    _httpStatusCode);
            });
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenEmptyStringAsTitle()
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
    public void ShouldThrowArgumentNullExceptionGivenNullDetail()
    {
        Assert.Throws<ArgumentNullException>(()
            => {
                Error.New(
                    _code,
                    _title,
                    null!,
                    _httpStatusCode);
            });
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenEmptyStringDetail()
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
    public void ShouldThrowArgumentNullExceptionGivenZeroAsfHttpStatusCode()
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
    public void ShouldCreateNewError()
    {
        Assert.NotNull(Error.New(_code, _title, _detail, _httpStatusCode));
    }

    [Theory]
    [InlineData("Identity.Argument.UserNameOrPasswordNotValid")]
    [InlineData("Identity.Argument.UserMustHaveAtLeastOneRole")]
    [InlineData("Identity.InvalidOperation.ItDoesNotExistAnyUser")]
    public void ShouldGetCodeGivenCreatedError(string code)
    {
        Error error = Error.New(code, _title, _detail, _httpStatusCode);
        Assert.Equal(code, error.GetCode());
    }

    [Theory]
    [InlineData("User was not deleted.")]
    [InlineData("The user must has at least one role assigned.")]
    [InlineData("User name or password aren't valid.")]
    public void ShouldGetTitleGivenCreatedError(string title)
    {
        Error error = Error.New(_code, title, _detail, _httpStatusCode);
        Assert.Equal(title, error.GetTitle());
    }

    [Theory]
    [InlineData("Because of internal error the user wasn't deleted, please contact with user administrator.")]
    [InlineData("The user doesn't have any role assigned and must have at least one.")]
    [InlineData("The user name or password wich were sent are not correct, either the user doesn't exist or password isn't correct.")]
    public void ShouldGetDetailGivenCreatedError(string detail)
    {
        Error error = Error.New(_code, _title, detail, _httpStatusCode);
        Assert.Equal(detail, error.GetDetail());
    }

    [Theory]
    [InlineData(200)]
    [InlineData(404)]
    [InlineData(500)]
    public void Should_GetHttpStatusCode_When_ErrorIsCreated(int httpStatusCode)
    {
        Error error = Error.New(_code, _title, _detail, httpStatusCode);
        Assert.Equal(httpStatusCode, error.GetHttpStatusCode());
    }

    [Fact]
    public void ShouldConvertToString()
    {
        Error error = Error.New(_code, _title, _detail, _httpStatusCode);
        string errorCode = error;
        Assert.Equal(error.GetCode(), errorCode);
    }

    [Fact]
    public void ShouldBeEqualGivenSameErrors()
    {
        Error none = Error.New("None", "None", "None", 200);
        Assert.True(none.Equals(Error.None));
    }

    [Fact]
    public void ShouldNotBeEqualGivenDifferentErrors()
    {
        Error error = Error.New(_code, _title, _detail, _httpStatusCode);
        Assert.False(error.Equals(Error.None));
    }

    [Fact]
    public void ShouldNotBeEqualGivenNullToCampareWith()
    {
        Error error = Error.New(_code, _title, _detail, _httpStatusCode);
        Assert.False(error.Equals(null));
    }

    [Fact]
    public void ShouldNotBeEqualGivenDifferentObjectToCompareWith()
    {
        Error error = Error.New(_code, _title, _detail, _httpStatusCode);
        Assert.False(error.Equals(new object()));
    }
}
