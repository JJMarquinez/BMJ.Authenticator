using BMJ.Authenticator.Domain.Common.Errors;

namespace BMJ.Authenticator.Domain.Common.Results;

public class Result<TValue> : Result
{
    private readonly TValue _value;

    private Result(TValue value, bool success, Error error)
        : base(success, error)
        => _value = value;

    public TValue GetValue() => _value;

    public static implicit operator Result<TValue>(TValue value) => Success(value);

    internal static Result<TValue> New(TValue value, bool success, Error error) => new(value, success, error);
}
