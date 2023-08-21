using BMJ.Authenticator.Adapter.Common;
using BMJ.Authenticator.Application.Common.Models;

namespace BMJ.Authenticator.Infrastructure.UnitTests.Common;

public class InfrastructureErrorTests
{
    private readonly string _codeArgumentPrefix = null!;
    private readonly string _codeInvalidOperationPrefix = null!;

    public InfrastructureErrorTests()
    {
        _codeArgumentPrefix = "Identity.Argument.";
        _codeInvalidOperationPrefix = "Identity.InvalidOperation.";
    }

    [Fact]
    public void ShouldBeUserNameOrPasswordNotValidError()
    {
        ErrorDto error = InfrastructureError.Identity.UserNameOrPasswordNotValid;
        string expectedCode = string.Concat(_codeInvalidOperationPrefix, nameof(InfrastructureError.Identity.UserNameOrPasswordNotValid));

        Assert.NotNull(error);
        Assert.Equal(expectedCode, error.Code);
        Assert.Equal("User name or password aren't valid.", error.Title);
        Assert.Equal("The user name or password wich were sent are not correct, either the user doesn't exist or password isn't correct.", error.Detail);
        Assert.Equal(409, error.HttpStatusCode);
    }

    [Fact]
    public void ShouldBeUserMustHaveAtLeastOneRoleError()
    {
        ErrorDto error = InfrastructureError.Identity.UserMustHaveAtLeastOneRole;
        string expectedCode = string.Concat(_codeInvalidOperationPrefix, nameof(InfrastructureError.Identity.UserMustHaveAtLeastOneRole));

        Assert.NotNull(error);
        Assert.Equal(expectedCode, error.Code);
        Assert.Equal("The user must has at least one role assigned.", error.Title);
        Assert.Equal("The user doesn't have any role assigned and must have at least one.", error.Detail);
        Assert.Equal(409, error.HttpStatusCode);
    }

    [Fact]
    public void ShouldBeUserWasNotDeletedError()
    {
        ErrorDto error = InfrastructureError.Identity.UserWasNotDeleted;
        string expectedCode = string.Concat(_codeInvalidOperationPrefix, nameof(InfrastructureError.Identity.UserWasNotDeleted));

        Assert.NotNull(error);
        Assert.Equal(expectedCode, error.Code);
        Assert.Equal("User was not deleted.", error.Title);
        Assert.Equal("Because of internal error the user wasn't deleted, please contact with user administrator.", error.Detail);
        Assert.Equal(409, error.HttpStatusCode);
    }

    [Fact]
    public void ShouldBeUserWasNotFoundError()
    {
        ErrorDto error = InfrastructureError.Identity.UserWasNotFound;
        string expectedCode = string.Concat(_codeArgumentPrefix, nameof(InfrastructureError.Identity.UserWasNotFound));

        Assert.NotNull(error);
        Assert.Equal(expectedCode, error.Code);
        Assert.Equal("User was not found.", error.Title);
        Assert.Equal("Any user isn't found with the data acquired.", error.Detail);
        Assert.Equal(404, error.HttpStatusCode);
    }

    [Fact]
    public void ShouldBeUserWasNotCreatedError()
    {
        ErrorDto error = InfrastructureError.Identity.UserWasNotCreated;
        string expectedCode = string.Concat(_codeInvalidOperationPrefix, nameof(InfrastructureError.Identity.UserWasNotCreated));

        Assert.NotNull(error);
        Assert.Equal(expectedCode, error.Code);
        Assert.Equal("User was not created.", error.Title);
        Assert.Equal("Because of internal error the user wasn't created, please contact with user administrator.", error.Detail);
        Assert.Equal(409, error.HttpStatusCode);
    }

    [Fact]
    public void ShouldBeItDoesNotExistAnyUserError()
    {
        ErrorDto error = InfrastructureError.Identity.ItDoesNotExistAnyUser;
        string expectedCode = string.Concat(_codeInvalidOperationPrefix, nameof(InfrastructureError.Identity.ItDoesNotExistAnyUser));

        Assert.NotNull(error);
        Assert.Equal(expectedCode, error.Code);
        Assert.Equal("It doesn't exist any user.", error.Title);
        Assert.Equal("There is no saved users to get.", error.Detail);
        Assert.Equal(404, error.HttpStatusCode);
    }

    [Fact]
    public void ShouldBeUserNameIsNotAvailableError()
    {
        ErrorDto error = InfrastructureError.Identity.UserNameIsNotAvailable;
        string expectedCode = string.Concat(_codeInvalidOperationPrefix, nameof(InfrastructureError.Identity.UserNameIsNotAvailable));

        Assert.NotNull(error);
        Assert.Equal(expectedCode, error.Code);
        Assert.Equal("User name is not available.", error.Title);
        Assert.Equal("The user name is already in use, please change the user name.", error.Detail);
        Assert.Equal(409, error.HttpStatusCode);
    }

    [Fact]
    public void ShouldBeUserWasNotUpdatedError()
    {
        ErrorDto error = InfrastructureError.Identity.UserWasNotUpdated;
        string expectedCode = string.Concat(_codeInvalidOperationPrefix, nameof(InfrastructureError.Identity.UserWasNotUpdated));

        Assert.NotNull(error);
        Assert.Equal(expectedCode, error.Code);
        Assert.Equal("User was not updated.", error.Title);
        Assert.Equal("Because of internal error the user wasn't updated, please contact with user administrator.", error.Detail);
        Assert.Equal(409, error.HttpStatusCode);
    }
}
