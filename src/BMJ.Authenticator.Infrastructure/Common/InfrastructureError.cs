using BMJ.Authenticator.Domain.Common;

namespace BMJ.Authenticator.Infrastructure.Common;

public class InfrastructureError
{
    public static class Identity
    {
        private static readonly string _codeArgumentPrefix = "Identity.Argument.";
        private static readonly string _codeInvalidOperationPrefix = "Identity.InvalidOperation.";

        public static readonly Error UserNameOrPasswordNotValid
            = Error.New(
                string.Concat(_codeArgumentPrefix, nameof(UserNameOrPasswordNotValid)), 
                "User name or password aren't valid.",
                "The user name or password wich were sent are not correct, either the user doesn't exist or password isn't correct.",
                400);

        public static readonly Error UserMustHaveAtLeastOneRole
            = Error.New(
                string.Concat(_codeInvalidOperationPrefix, nameof(UserMustHaveAtLeastOneRole)),
                "The user must has at least one role assigned.",
                "The user doesn't have any role assigned and must have at least one.",
                409);

        public static readonly Error UserWasNotDeleted
            = Error.New(
                string.Concat(_codeInvalidOperationPrefix, nameof(UserWasNotDeleted)),
                "User was not deleted.",
                "Because of internal error the user wasn't deleted, please contact with user administrator.",
                409);

        public static readonly Error UserWasNotFound
            = Error.New(
                string.Concat(_codeArgumentPrefix, nameof(UserWasNotFound)),
                "User was not found.",
                "Any user isn't found with the data acquired.",
                404);

        public static readonly Error UserWasNotCreated
            = Error.New(
                string.Concat(_codeInvalidOperationPrefix, nameof(UserWasNotDeleted)),
                "User was not created.",
                "Because of internal error the user wasn't created, please contact with user administrator.",
                404);

        public static readonly Error ItDoesNotExistAnyUser
            = Error.New(
                string.Concat(_codeInvalidOperationPrefix, nameof(ItDoesNotExistAnyUser)),
                "It doesn't exist any user.",
                "There is no saved users to get",
                404);
    }
}
