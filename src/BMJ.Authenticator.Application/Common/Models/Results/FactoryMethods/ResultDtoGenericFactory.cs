namespace BMJ.Authenticator.Application.Common.Models.Results.FactoryMethods;

public class ResultDtoGenericFactory : IResultDtoGenericFactory
{
    public ResultDto<TValue?> FactoryMethod<TValue>(TValue? value, ErrorDto errorDto)
        => errorDto == ErrorDto.None
        ? ResultDto<TValue?>.MakeSuccess(value)
        : ResultDto<TValue?>.MakeFailure<TValue?>(errorDto);
}
