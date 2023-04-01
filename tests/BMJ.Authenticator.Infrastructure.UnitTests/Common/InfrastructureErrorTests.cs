using BMJ.Authenticator.Domain.Common.Errors;
using BMJ.Authenticator.Infrastructure.Common;

namespace BMJ.Authenticator.Infrastructure.UnitTests.Common
{
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
            Error error = InfrastructureError.Identity.UserNameOrPasswordNotValid;
            string expectedCode = string.Concat(_codeInvalidOperationPrefix, nameof(InfrastructureError.Identity.UserNameOrPasswordNotValid));

            Assert.NotNull(error);
            Assert.Equal(expectedCode, error.GetCode());
            Assert.Equal("User name or password aren't valid.", error.GetTitle());
            Assert.Equal("The user name or password wich were sent are not correct, either the user doesn't exist or password isn't correct.", error.GetDetail());
            Assert.Equal(409, error.GetHttpStatusCode());
        }

        [Fact]
        public void ShouldBeUserMustHaveAtLeastOneRoleError()
        {
            Error error = InfrastructureError.Identity.UserMustHaveAtLeastOneRole;
            string expectedCode = string.Concat(_codeInvalidOperationPrefix, nameof(InfrastructureError.Identity.UserMustHaveAtLeastOneRole));

            Assert.NotNull(error);
            Assert.Equal(expectedCode, error.GetCode());
            Assert.Equal("The user must has at least one role assigned.", error.GetTitle());
            Assert.Equal("The user doesn't have any role assigned and must have at least one.", error.GetDetail());
            Assert.Equal(409, error.GetHttpStatusCode());
        }

        [Fact]
        public void ShouldBeUserWasNotDeletedError()
        {
            Error error = InfrastructureError.Identity.UserWasNotDeleted;
            string expectedCode = string.Concat(_codeInvalidOperationPrefix, nameof(InfrastructureError.Identity.UserWasNotDeleted));

            Assert.NotNull(error);
            Assert.Equal(expectedCode, error.GetCode());
            Assert.Equal("User was not deleted.", error.GetTitle());
            Assert.Equal("Because of internal error the user wasn't deleted, please contact with user administrator.", error.GetDetail());
            Assert.Equal(409, error.GetHttpStatusCode());
        }

        [Fact]
        public void ShouldBeUserWasNotFoundError()
        {
            Error error = InfrastructureError.Identity.UserWasNotFound;
            string expectedCode = string.Concat(_codeArgumentPrefix, nameof(InfrastructureError.Identity.UserWasNotFound));

            Assert.NotNull(error);
            Assert.Equal(expectedCode, error.GetCode());
            Assert.Equal("User was not found.", error.GetTitle());
            Assert.Equal("Any user isn't found with the data acquired.", error.GetDetail());
            Assert.Equal(404, error.GetHttpStatusCode());
        }

        [Fact]
        public void ShouldBeUserWasNotCreatedError()
        {
            Error error = InfrastructureError.Identity.UserWasNotCreated;
            string expectedCode = string.Concat(_codeInvalidOperationPrefix, nameof(InfrastructureError.Identity.UserWasNotCreated));

            Assert.NotNull(error);
            Assert.Equal(expectedCode, error.GetCode());
            Assert.Equal("User was not created.", error.GetTitle());
            Assert.Equal("Because of internal error the user wasn't created, please contact with user administrator.", error.GetDetail());
            Assert.Equal(409, error.GetHttpStatusCode());
        }

        [Fact]
        public void ShouldBeItDoesNotExistAnyUserError()
        {
            Error error = InfrastructureError.Identity.ItDoesNotExistAnyUser;
            string expectedCode = string.Concat(_codeInvalidOperationPrefix, nameof(InfrastructureError.Identity.ItDoesNotExistAnyUser));

            Assert.NotNull(error);
            Assert.Equal(expectedCode, error.GetCode());
            Assert.Equal("It doesn't exist any user.", error.GetTitle());
            Assert.Equal("There is no saved users to get.", error.GetDetail());
            Assert.Equal(404, error.GetHttpStatusCode());
        }

        [Fact]
        public void ShouldBeUserNameIsNotAvailableError()
        {
            Error error = InfrastructureError.Identity.UserNameIsNotAvailable;
            string expectedCode = string.Concat(_codeInvalidOperationPrefix, nameof(InfrastructureError.Identity.UserNameIsNotAvailable));

            Assert.NotNull(error);
            Assert.Equal(expectedCode, error.GetCode());
            Assert.Equal("User name is not available.", error.GetTitle());
            Assert.Equal("The user name is already in use, please change the user name.", error.GetDetail());
            Assert.Equal(409, error.GetHttpStatusCode());
        }

        [Fact]
        public void ShouldBeUserWasNotUpdatedError()
        {
            Error error = InfrastructureError.Identity.UserWasNotUpdated;
            string expectedCode = string.Concat(_codeInvalidOperationPrefix, nameof(InfrastructureError.Identity.UserWasNotUpdated));

            Assert.NotNull(error);
            Assert.Equal(expectedCode, error.GetCode());
            Assert.Equal("User was not updated.", error.GetTitle());
            Assert.Equal("Because of internal error the user wasn't updated, please contact with user administrator.", error.GetDetail());
            Assert.Equal(409, error.GetHttpStatusCode());
        }
    }
}
