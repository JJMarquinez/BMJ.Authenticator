using BMJ.Authenticator.Domain.Common.Errors;

namespace BMJ.Authenticator.Domain.Common.Results.FactoryMethods;

public interface IResultFactory
{
    Result FactoryMethod(Error error);
}
