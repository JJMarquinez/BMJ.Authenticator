using BMJ.Authenticator.Application.Common.Models;

namespace BMJ.Authenticator.Adapter.Common;

public class InfrastructureError
{
    public static class Identity
    {
        private static readonly string _codeArgumentPrefix = "Identity.Argument.";
        private static readonly string _codeInvalidOperationPrefix = "Identity.InvalidOperation.";

        public static readonly ErrorDto UserNameOrPasswordNotValid
            = new ErrorDto
            {
                Code = string.Concat(_codeInvalidOperationPrefix, nameof(UserNameOrPasswordNotValid)),
                Title = "User name or password aren't valid.",
                Detail = "The user name or password wich were sent are not correct, either the user doesn't exist or password isn't correct.",
                HttpStatusCode = 409
            };
        
        public static readonly ErrorDto UserMustHaveAtLeastOneRole
            = new ErrorDto
            {
                Code = string.Concat(_codeInvalidOperationPrefix, nameof(UserMustHaveAtLeastOneRole)),
                Title = "The user must has at least one role assigned.",
                Detail = "The user doesn't have any role assigned and must have at least one.",
                HttpStatusCode = 409
            };

        public static readonly ErrorDto UserWasNotDeleted
            = new ErrorDto
            {
                Code = string.Concat(_codeInvalidOperationPrefix, nameof(UserWasNotDeleted)),
                Title = "User was not deleted.",
                Detail = "Because of internal error the user wasn't deleted, please contact with user administrator.",
                HttpStatusCode = 409
            };

        public static readonly ErrorDto UserWasNotFound
            = new ErrorDto
            {
                Code = string.Concat(_codeArgumentPrefix, nameof(UserWasNotFound)),
                Title = "User was not found.",
                Detail = "Any user isn't found with the data acquired.",
                HttpStatusCode = 404
            };

        public static readonly ErrorDto UserWasNotCreated
            = new ErrorDto
            {
                Code = string.Concat(_codeInvalidOperationPrefix, nameof(UserWasNotCreated)),
                Title = "User was not created.",
                Detail = "Because of internal error the user wasn't created, please contact with user administrator.",
                HttpStatusCode = 409
            };

        public static readonly ErrorDto ItDoesNotExistAnyUser
            = new ErrorDto
            {
                Code = string.Concat(_codeInvalidOperationPrefix, nameof(ItDoesNotExistAnyUser)),
                Title = "It doesn't exist any user.",
                Detail = "There is no saved users to get.",
                HttpStatusCode = 404
            };

        public static readonly ErrorDto UserNameIsNotAvailable
            = new ErrorDto
            {
                Code = string.Concat(_codeInvalidOperationPrefix, nameof(UserNameIsNotAvailable)),
                Title = "User name is not available.",
                Detail = "The user name is already in use, please change the user name.",
                HttpStatusCode = 409
            };

        public static readonly ErrorDto UserWasNotUpdated
            = new ErrorDto
            {
                Code = string.Concat(_codeInvalidOperationPrefix, nameof(UserWasNotUpdated)),
                Title = "User was not updated.",
                Detail = "Because of internal error the user wasn't updated, please contact with user administrator.",
                HttpStatusCode = 409
            };
    }
}
