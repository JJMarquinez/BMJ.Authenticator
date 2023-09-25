using BMJ.Authenticator.Domain.Common.Errors;

namespace BMJ.Authenticator.Domain.Common.Results;

public class Result
{
    private readonly bool _success;

    private protected Result(bool success, Error error)
    {
        Ensure.Argument.NotNull(error, string.Format("{0} cannot be null.", nameof(error)));
        Ensure.Argument.IsNot(!success && error == Error.None, "The failure result cannot be implemented with no error");
        _success = success;
        Error = error;
    }

    public bool IsSuccess() => _success;
    public bool IsFailure() => !IsSuccess();
    public Error Error { get; }
    internal static Result MakeSuccess() => new(true, Error.None);
    internal static Result MakeFailure(Error error) => new(false, error);
    internal static Result<TValue> MakeSuccess<TValue>(TValue value) => Result<TValue>.NewInstance(value, true, Error.None);
    internal static Result<TValue?> MakeFailure<TValue>(Error error) => Result<TValue?>.NewInstance(default, false, error);
}
