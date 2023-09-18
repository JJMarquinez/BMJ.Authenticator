using BMJ.Authenticator.Domain.Common.Errors;

namespace BMJ.Authenticator.Domain.Common.Results.FactoryMethods;

public interface IResultCreator
{
    Result CreateSuccessResult();
    Result CreateFailureResult(Error error);
    Result<TValue> CreateSuccessResult<TValue>(TValue value);
    Result<TValue?> CreateFailureResult<TValue>(Error error);
}
