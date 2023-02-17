namespace BMJ.Authenticator.Domain.Common.Results;

public class Result
{
    private readonly bool _success;
    private readonly Error _error;

    private protected Result(bool success, Error error)
    {
        Ensure.Argument.NotNull(error, nameof(error));
        _success = success;
        _error = error;
    }

    public bool IsSuccess() => _success;
    public bool IsFailure() => !IsSuccess();
    public Error GetError() => _error;
    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    public static Result<TValue> Success<TValue>(TValue value) => Result<TValue>.New(value, true, Error.None);
    public static Result<TValue?> Failure<TValue>(Error error) => Result<TValue?>.New(default, false, error);
}
