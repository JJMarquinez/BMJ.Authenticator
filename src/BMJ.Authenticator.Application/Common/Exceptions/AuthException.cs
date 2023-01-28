using BMJ.Authenticator.Application.Common.Models;
using FluentValidation.Results;

namespace BMJ.Authenticator.Application.Common.Exceptions;

public class AuthException : Exception
{
    private readonly Error error;
    private AuthException(Error error)
        : base("Some internal error has occurred.")
    {
        this.error = error;
    }

    public static AuthException New(Error error)
        => new (error);

    public Error GetError() => error;
}
