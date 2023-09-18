namespace BMJ.Authenticator.Application.Common.Models.Results.FactoryMethods;

public interface IResultDtoFactory
{
    ResultDto FactoryMethod(ErrorDto errorDto);
}
