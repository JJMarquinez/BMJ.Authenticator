using BMJ.Authenticator.Domain.Common.Errors;
using BMJ.Authenticator.Domain.Common.Errors.Builders;

namespace BMJ.Authenticator.Domain.UnitTests.Common.Errors;

public class ErrorTests
{
    private readonly IErrorBuilder _errorBuilder;
    private readonly string _code;
    private readonly string _title;
    private readonly string _detail;
    private readonly int _httpStatusCode;

    public ErrorTests()
    {
        _errorBuilder = new ErrorBuilder();
        _code = "Identity.InvalidOperation.UserNameOrPasswordNotValid";
        _title = "User name or password aren't valid.";
        _detail = "The user name or password wich were sent are not correct, either the user doesn't exist or password isn't correct.";
        _httpStatusCode = 409;
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullCode()
    {
        Assert.Throws<ArgumentNullException>(() => _errorBuilder.WithCode(null!).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build());
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenEmptyStringAsCode()
    {
        Assert.Throws<ArgumentException>(() => _errorBuilder.WithCode(string.Empty).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build());
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullTitle()
    {
        Assert.Throws<ArgumentNullException>(() => _errorBuilder.WithCode(_code).WithTitle(null!).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build());
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenEmptyStringAsTitle()
    {
        Assert.Throws<ArgumentException>(() => _errorBuilder.WithCode(_code).WithTitle(string.Empty).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build());
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullDetail()
    {
        Assert.Throws<ArgumentNullException>(() => _errorBuilder.WithCode(_code).WithTitle(_title).WithDetail(null!).WithHttpStatusCode(_httpStatusCode).Build());
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenEmptyStringDetail()
    {
        Assert.Throws<ArgumentException>(() => _errorBuilder.WithCode(_code).WithTitle(_title).WithDetail(string.Empty).WithHttpStatusCode(_httpStatusCode).Build());
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenZeroAsfHttpStatusCode()
    {
        Assert.Throws<ArgumentException>(() => _errorBuilder.WithCode(_code).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(0).Build());
    }

    [Fact]
    public void ShouldCreateNewError()
    {
        Assert.NotNull(_errorBuilder.WithCode(_code).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build());
    }

    [Theory]
    [InlineData("Identity.Argument.UserNameOrPasswordNotValid")]
    [InlineData("Identity.Argument.UserMustHaveAtLeastOneRole")]
    [InlineData("Identity.InvalidOperation.ItDoesNotExistAnyUser")]
    public void ShouldGetCodeGivenCreatedError(string code)
    {
        Error error = _errorBuilder.WithCode(code).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build();
        Assert.Equal(code, error.Code);
    }

    [Theory]
    [InlineData("User was not deleted.")]
    [InlineData("The user must has at least one role assigned.")]
    [InlineData("User name or password aren't valid.")]
    public void ShouldGetTitleGivenCreatedError(string title)
    {
        Error error = _errorBuilder.WithCode(_code).WithTitle(title).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build();
        Assert.Equal(title, error.Title);
    }

    [Theory]
    [InlineData("Because of internal error the user wasn't deleted, please contact with user administrator.")]
    [InlineData("The user doesn't have any role assigned and must have at least one.")]
    [InlineData("The user name or password wich were sent are not correct, either the user doesn't exist or password isn't correct.")]
    public void ShouldGetDetailGivenCreatedError(string detail)
    {
        Error error = _errorBuilder.WithCode(_code).WithTitle(_title).WithDetail(detail).WithHttpStatusCode(_httpStatusCode).Build();
        Assert.Equal(detail, error.Detail);
    }

    [Theory]
    [InlineData(200)]
    [InlineData(404)]
    [InlineData(500)]
    public void Should_GetHttpStatusCode_When_ErrorIsCreated(int httpStatusCode)
    {
        Error error = _errorBuilder.WithCode(_code).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(httpStatusCode).Build();
        Assert.Equal(httpStatusCode, error.HttpStatusCode);
    }

    [Fact]
    public void ShouldConvertToString()
    {
        Error error = _errorBuilder.WithCode(_code).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build();
        string errorCode = error;
        Assert.Equal(error.Code, errorCode);
    }

    [Fact]
    public void ShouldBeEqualGivenSameErrors()
    {
        string noneValue = "None";
        Error none = _errorBuilder.WithCode(noneValue).WithTitle(noneValue).WithDetail(noneValue).WithHttpStatusCode(200).Build();
        Assert.True(none.Equals(Error.None));
    }

    [Fact]
    public void ShouldNotBeEqualGivenDifferentErrors()
    {
        Error error = _errorBuilder.WithCode(_code).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build();
        Assert.False(error.Equals(Error.None));
    }

    [Fact]
    public void ShouldNotBeEqualGivenNullToCampareWith()
    {
        Error error = _errorBuilder.WithCode(_code).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build();
        Assert.False(error.Equals(null));
    }

    [Fact]
    public void ShouldNotBeEqualGivenDifferentObjectToCompareWith()
    {
        Error error = _errorBuilder.WithCode(_code).WithTitle(_title).WithDetail(_detail).WithHttpStatusCode(_httpStatusCode).Build();
        Assert.False(error.Equals(new object()));
    }
}
