using BMJ.Authenticator.Domain.Common.Errors;

namespace BMJ.Authenticator.Adapter.Common;

public class InfrastructureError
{
    public static class Identity
    {
        private static readonly string _codeArgumentPrefix = "Identity.Argument.";
        private static readonly string _codeInvalidOperationPrefix = "Identity.InvalidOperation.";

        public static readonly Error UserNameOrPasswordNotValid
            = Error.Builder()
            .WithCode(string.Concat(_codeInvalidOperationPrefix, nameof(UserNameOrPasswordNotValid)))
            .WithTitle("User name or password aren't valid.")
            .WithDetail("The user name or password wich were sent are not correct, either the user doesn't exist or password isn't correct.")
            .WithHttpStatusCode(409)
            .Build();

        public static readonly Error UserMustHaveAtLeastOneRole
            = Error.Builder()
            .WithCode(string.Concat(_codeInvalidOperationPrefix, nameof(UserMustHaveAtLeastOneRole)))
            .WithTitle("The user must has at least one role assigned.")
            .WithDetail("The user doesn't have any role assigned and must have at least one.")
            .WithHttpStatusCode(409)
            .Build();

        public static readonly Error UserWasNotDeleted
            = Error.Builder()
            .WithCode(string.Concat(_codeInvalidOperationPrefix, nameof(UserWasNotDeleted)))
            .WithTitle("User was not deleted.")
            .WithDetail("Because of internal error the user wasn't deleted, please contact with user administrator.")
            .WithHttpStatusCode(409)
            .Build();

        public static readonly Error UserWasNotFound
            = Error.Builder()
            .WithCode(string.Concat(_codeArgumentPrefix, nameof(UserWasNotFound)))
            .WithTitle("User was not found.")
            .WithDetail("Any user isn't found with the data acquired.")
            .WithHttpStatusCode(404)
            .Build();

        public static readonly Error UserWasNotCreated
            = Error.Builder()
            .WithCode(string.Concat(_codeInvalidOperationPrefix, nameof(UserWasNotCreated)))
            .WithTitle("User was not created.")
            .WithDetail("Because of internal error the user wasn't created, please contact with user administrator.")
            .WithHttpStatusCode(409)
            .Build();

        public static readonly Error ItDoesNotExistAnyUser
            = Error.Builder()
            .WithCode(string.Concat(_codeInvalidOperationPrefix, nameof(ItDoesNotExistAnyUser)))
            .WithTitle("It doesn't exist any user.")
            .WithDetail("There is no saved users to get.")
            .WithHttpStatusCode(404)
            .Build();

        public static readonly Error UserNameIsNotAvailable
            = Error.Builder()
            .WithCode(string.Concat(_codeInvalidOperationPrefix, nameof(UserNameIsNotAvailable)))
            .WithTitle("User name is not available.")
            .WithDetail("The user name is already in use, please change the user name.")
            .WithHttpStatusCode(409)
            .Build();

        public static readonly Error UserWasNotUpdated
            = Error.Builder()
            .WithCode(string.Concat(_codeInvalidOperationPrefix, nameof(UserWasNotUpdated)))
            .WithTitle("User was not updated.")
            .WithDetail("Because of internal error the user wasn't updated, please contact with user administrator.")
            .WithHttpStatusCode(409)
            .Build();
    }
}
