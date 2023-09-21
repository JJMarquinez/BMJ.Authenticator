using BMJ.Authenticator.Application.Common.Models.Errors;

namespace BMJ.Authenticator.Application.Common.Models.Results.FactoryMethods;

public class ResultDtoFactory : IResultDtoFactory
{
    public ResultDto FactoryMethod(ErrorDto errorDto)
        => errorDto == ErrorDto.None
        ? ResultDto.MakeSuccess()
        : ResultDto.MakeFailure(errorDto);
}
