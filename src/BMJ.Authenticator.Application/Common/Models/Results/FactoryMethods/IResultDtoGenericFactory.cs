using BMJ.Authenticator.Application.Common.Models.Errors;

namespace BMJ.Authenticator.Application.Common.Models.Results.FactoryMethods;

public interface IResultDtoGenericFactory
{
    ResultDto<TValue?> FactoryMethod<TValue>(TValue? value, ErrorDto errorDto);
}
