using BMJ.Authenticator.Domain.Common.Errors;

namespace BMJ.Authenticator.Domain.Common.Results.Builders;

public class ResultGenericBuilder : IResultGenericBuilder
{
    public Result<TValue?> BuildFailure<TValue>(Error error) => Result.MakeFailure<TValue>(error);

    public Result<TValue> BuildSuccess<TValue>(TValue value) => Result.MakeSuccess(value);
}
