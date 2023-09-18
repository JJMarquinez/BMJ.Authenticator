using BMJ.Authenticator.Domain.Common;

namespace BMJ.Authenticator.Application.Common.Models.Results.FactoryMethods;

public class ResultDtoCreator : IResultDtoCreator
{
    private readonly IResultDtoFactory _resultDtoFactory;
    private readonly IResultDtoGenericFactory _resultDtoGenericFactory;

    public ResultDtoCreator(IResultDtoFactory resultDtoFactory, IResultDtoGenericFactory resultDtoGenericFactory)
    {
        _resultDtoFactory = resultDtoFactory;
        _resultDtoGenericFactory = resultDtoGenericFactory;
    }

    public ResultDto CreateFailureResult(ErrorDto error)
    {
        Ensure.Argument.IsNot(error == ErrorDto.None, "It is not possible to create failure result with no error");
        return _resultDtoFactory.FactoryMethod(error);
    }

    public ResultDto<TValue?> CreateFailureResult<TValue>(ErrorDto error)
    {
        Ensure.Argument.IsNot(error == ErrorDto.None, "It is not possible to create failure result with no error");
        return _resultDtoGenericFactory.FactoryMethod(default(TValue), error);
    }

    public ResultDto CreateSuccessResult()
        => _resultDtoFactory.FactoryMethod(ErrorDto.None);

    public ResultDto<TValue> CreateSuccessResult<TValue>(TValue value)
        => _resultDtoGenericFactory.FactoryMethod(value, ErrorDto.None)!;
}
