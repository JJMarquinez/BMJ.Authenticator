using BMJ.Authenticator.Domain.Common.Errors;

namespace BMJ.Authenticator.Domain.Common.Results.FactoryMethods;

public class ResultCreator : IResultCreator
{
    private readonly IResultFactory _resultFactory;
    private readonly IResultGenericFactory _resultGenericFactory;

    public ResultCreator(IResultFactory resultFactory, IResultGenericFactory resultGenericFactory)
    {
        _resultFactory = resultFactory;
        _resultGenericFactory = resultGenericFactory;
    }

    public Result CreateFailureResult(Error error)
    {
        Ensure.Argument.IsNot(error == Error.None, "It is not possible to create failure result with no error");
        return _resultFactory.FactoryMethod(error);
    }

    public Result<TValue?> CreateFailureResult<TValue>(Error error)
    {
        Ensure.Argument.IsNot(error == Error.None, "It is not possible to create failure result with no error");
        return _resultGenericFactory.FactoryMethod(default(TValue), error);
    }

    public Result CreateSuccessResult()
        => _resultFactory.FactoryMethod(Error.None);

    public Result<TValue> CreateSuccessResult<TValue>(TValue value)
        => _resultGenericFactory.FactoryMethod(value, Error.None)!;
}
