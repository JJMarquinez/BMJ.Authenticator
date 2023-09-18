using BMJ.Authenticator.Domain.Common.Errors;

namespace BMJ.Authenticator.Domain.Common.Results.FactoryMethods;

public class ResultGenericFactory : IResultGenericFactory
{
    public Result<TValue?> FactoryMethod<TValue>(TValue? value, Error error)
        => error == Error.None
        ? Result.MakeSuccess(value)
        : Result.MakeFailure<TValue>(error);
}
