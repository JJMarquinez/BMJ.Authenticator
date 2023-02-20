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
                "The user identification isn't link to any user.",
                404);

        public static readonly Error UserWasNotCreated
            = Error.New(
                string.Concat(_codeInvalidOperationPrefix, nameof(UserWasNotDeleted)),
                "User was not created.",
                "Because of internal error the user wasn't created, please contact with user administrator.",
                404);
    }
}
