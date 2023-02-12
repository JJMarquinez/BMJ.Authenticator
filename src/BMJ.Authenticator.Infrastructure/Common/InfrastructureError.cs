using BMJ.Authenticator.Domain.Common;

namespace BMJ.Authenticator.Infrastructure.Common;

public class InfrastructureError
{
    public static class Identity
    {
        private static readonly string _codeArgumentPrefix = "Identity.Argument.";
        private static readonly string _codeInvalidOperationPrefix = "Identity.InvalidOperation.";

        public static readonly Error UserNameOrPasswordNotValid
            = Error.New(string.Concat(_codeArgumentPrefix, nameof(UserNameOrPasswordNotValid)), 
                "User name or pasword not valid.");

        public static readonly Error UserWasNotDeleted
            = Error.New(string.Concat(_codeInvalidOperationPrefix, nameof(UserWasNotDeleted)),
                "User was not deleted.");

        public static readonly Error UserWasNotFound
            = Error.New(string.Concat(_codeArgumentPrefix, nameof(UserWasNotFound)),
                "User was not found.");

        public static readonly Error UserWasNotCreated
            = Error.New(string.Concat(_codeInvalidOperationPrefix, nameof(UserWasNotDeleted)),
                "User was not created.");
    }
}
