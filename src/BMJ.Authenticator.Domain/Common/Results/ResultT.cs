using BMJ.Authenticator.Domain.Common.Errors;

namespace BMJ.Authenticator.Domain.Common.Results;

public class Result<TValue> : Result
{
    private Result(TValue value, bool success, Error error)
        : base(success, error)
    {
        Ensure.Argument.IsNot(value is null && error == Error.None, "The result cannot be implemented with no value and no error");
        Value = value;
    }

    public TValue Value { get; }

    public static implicit operator Result<TValue>(TValue value) => MakeSuccess(value);

    internal static Result<TValue> NewInstance(TValue value, bool success, Error error) => new(value, success, error);
}
