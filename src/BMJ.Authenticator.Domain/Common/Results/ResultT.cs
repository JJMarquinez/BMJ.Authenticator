using BMJ.Authenticator.Domain.Common.Errors;

namespace BMJ.Authenticator.Domain.Common.Results;

public class Result<TValue> : Result
{
    private Result(TValue value, bool success, Error error)
        : base(success, error)
        => Value = value;

    public TValue Value { get; }

    public static implicit operator Result<TValue>(TValue value) => Success(value);

    internal static Result<TValue> New(TValue value, bool success, Error error) => new(value, success, error);
}
