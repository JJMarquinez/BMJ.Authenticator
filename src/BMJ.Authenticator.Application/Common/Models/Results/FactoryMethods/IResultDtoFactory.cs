using BMJ.Authenticator.Application.Common.Models.Errors;

namespace BMJ.Authenticator.Application.Common.Models.Results.FactoryMethods;

public interface IResultDtoFactory
{
    ResultDto FactoryMethod(ErrorDto errorDto);
}
