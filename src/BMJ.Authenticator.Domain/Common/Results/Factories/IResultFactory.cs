using BMJ.Authenticator.Domain.Common.Errors;

namespace BMJ.Authenticator.Domain.Common.Results.Factories;

public interface IResultFactory
{
    Result FactoryMethod(Error error);
}
