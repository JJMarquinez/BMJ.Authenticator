namespace BMJ.Authenticator.Application.Common.Models.Results.FactoryMethods;

public interface IResultDtoCreator
{
    ResultDto CreateSuccessResult();
    ResultDto CreateFailureResult(ErrorDto error);
    ResultDto<TValue> CreateSuccessResult<TValue>(TValue value);
    ResultDto<TValue?> CreateFailureResult<TValue>(ErrorDto error);
}
