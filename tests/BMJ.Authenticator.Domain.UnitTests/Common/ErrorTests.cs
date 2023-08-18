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
        Assert.Throws<ArgumentNullException>(() => Error.Builder().WithCode(null!).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build());
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenEmptyStringAsCode()
    {
        Assert.Throws<ArgumentException>(() => Error.Builder().WithCode(string.Empty).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build());
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullTitle()
    {
        Assert.Throws<ArgumentNullException>(() => Error.Builder().WithCode(_code).WithTitle(null!).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build());
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenEmptyStringAsTitle()
    {
        Assert.Throws<ArgumentException>(() => Error.Builder().WithCode(_code).WithTitle(string.Empty).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build());
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullDetail()
    {
        Assert.Throws<ArgumentNullException>(() => Error.Builder().WithCode(_code).WithTitle(_title).WithDetail(null!).WithHttpStatusCode(_httpStatusCode).Build());
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenEmptyStringDetail()
    {
        Assert.Throws<ArgumentException>(() => Error.Builder().WithCode(_code).WithTitle(_title).WithDetail(string.Empty).WithHttpStatusCode(_httpStatusCode).Build());
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenZeroAsfHttpStatusCode()
    {
        Assert.Throws<ArgumentException>(() => Error.Builder().WithCode(_code).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(0).Build());
    }

    [Fact]
    public void ShouldCreateNewError()
    {
        Assert.NotNull(Error.Builder().WithCode(_code).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build());
    }

    [Theory]
    [InlineData("Identity.Argument.UserNameOrPasswordNotValid")]
    [InlineData("Identity.Argument.UserMustHaveAtLeastOneRole")]
    [InlineData("Identity.InvalidOperation.ItDoesNotExistAnyUser")]
    public void ShouldGetCodeGivenCreatedError(string code)
    {
        Error error = Error.Builder().WithCode(code).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build();
        Assert.Equal(code, error.GetCode());
    }

    [Theory]
    [InlineData("User was not deleted.")]
    [InlineData("The user must has at least one role assigned.")]
    [InlineData("User name or password aren't valid.")]
    public void ShouldGetTitleGivenCreatedError(string title)
    {
        Error error = Error.Builder().WithCode(_code).WithTitle(title).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build();
        Assert.Equal(title, error.GetTitle());
    }

    [Theory]
    [InlineData("Because of internal error the user wasn't deleted, please contact with user administrator.")]
    [InlineData("The user doesn't have any role assigned and must have at least one.")]
    [InlineData("The user name or password wich were sent are not correct, either the user doesn't exist or password isn't correct.")]
    public void ShouldGetDetailGivenCreatedError(string detail)
    {
        Error error = Error.Builder().WithCode(_code).WithTitle(_title).WithDetail(detail).WithHttpStatusCode(_httpStatusCode).Build();
        Assert.Equal(detail, error.GetDetail());
    }

    [Theory]
    [InlineData(200)]
    [InlineData(404)]
    [InlineData(500)]
    public void Should_GetHttpStatusCode_When_ErrorIsCreated(int httpStatusCode)
    {
        Error error = Error.Builder().WithCode(_code).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(httpStatusCode).Build();
        Assert.Equal(httpStatusCode, error.GetHttpStatusCode());
    }

    [Fact]
    public void ShouldConvertToString()
    {
        Error error = Error.Builder().WithCode(_code).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build();
        string errorCode = error;
        Assert.Equal(error.GetCode(), errorCode);
    }

    [Fact]
    public void ShouldBeEqualGivenSameErrors()
    {
        string noneValue = "None";
        Error none = Error.Builder().WithCode(noneValue).WithTitle(noneValue).WithDetail(noneValue).WithHttpStatusCode(200).Build();
        Assert.True(none.Equals(Error.None));
    }

    [Fact]
    public void ShouldNotBeEqualGivenDifferentErrors()
    {
        Error error = Error.Builder().WithCode(_code).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build();
        Assert.False(error.Equals(Error.None));
    }

    [Fact]
    public void ShouldNotBeEqualGivenNullToCampareWith()
    {
        Error error = Error.Builder().WithCode(_code).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build();
        Assert.False(error.Equals(null));
    }

    [Fact]
    public void ShouldNotBeEqualGivenDifferentObjectToCompareWith()
    {
        Error error = Error.Builder().WithCode(_code).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build();
        Assert.False(error.Equals(new object()));
    }
}
