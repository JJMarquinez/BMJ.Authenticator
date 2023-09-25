using BMJ.Authenticator.Application.Common.Models.Errors;

namespace BMJ.Authenticator.Application.Common.Models.Results.Builders;

public class ResultDtoBuilder : IResultDtoBuilder, IResultDtoErrorBuilder
{
    private ErrorDto _errorDto = null!;
    public ResultDto Build() => ResultDto.MakeFailure(_errorDto);

    public ResultDto BuildSuccess() => ResultDto.MakeSuccess();

    public IResultDtoErrorBuilder WithError(ErrorDto errorDto)
    {
        _errorDto = errorDto;
        return this;
    }
}
