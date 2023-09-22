using BMJ.Authenticator.Domain.Common.Errors;

namespace BMJ.Authenticator.Domain.Common.Results.Factories;

public class ResultFactory : IResultFactory
{
    public Result FactoryMethod(Error error)
        => error == Error.None 
        ? Result.MakeSuccess() 
        : Result.MakeFailure(error);
}
