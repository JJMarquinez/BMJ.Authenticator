using BMJ.Authenticator.Domain.Common.Errors;

namespace BMJ.Authenticator.Domain.Common.Results.Factories;

public interface IResultGenericFactory
{
    Result<TValue?> FactoryMethod<TValue>(TValue? value, Error error);
}
