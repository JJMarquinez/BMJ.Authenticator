using System.Net;

namespace BMJ.Authenticator.Application.Common.Models;

internal class AuthErrors
{
    public static readonly Error FirstExempleError
        = Error.New(
            "auth00001",
            new string[] { "Desc error", "Desc error two" },
            ErrorType.Functional,
            HttpStatusCode.PreconditionRequired);
}
