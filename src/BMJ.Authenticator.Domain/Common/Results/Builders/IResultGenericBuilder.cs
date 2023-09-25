using BMJ.Authenticator.Domain.Common.Errors;

namespace BMJ.Authenticator.Domain.Common.Results.Builders;
public interface IResultGenericBuilder
{
    Result<TValue> BuildSuccess<TValue>(TValue value);
    Result<TValue?> BuildFailure<TValue>(Error error); 
}
