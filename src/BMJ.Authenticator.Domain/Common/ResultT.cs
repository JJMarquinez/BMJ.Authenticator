namespace BMJ.Authenticator.Domain.Common;

public class Result<TValue> : Result
{
    private readonly TValue _value;

    private Result(TValue value, bool success, Error error)
        : base(success, error) 
        => _value = value;

    public TValue Value => IsSuccess() ? _value : throw new InvalidDataException("The value of a failure result can not be accessed.");

    public static implicit operator Result<TValue>(TValue value) => Success(value);

    internal static Result<TValue> New(TValue value, bool success, Error error) => new(value, success, error);
}
