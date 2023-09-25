using BMJ.Authenticator.Application.Common.Models.Errors;

namespace BMJ.Authenticator.Application.Common.Models.Results.Builders;

public class ResultDtoGenericBuilder : IResultDtoGenericBuilder
{
    public ResultDto<TValue?> BuildFailure<TValue>(ErrorDto error) => ResultDto<TValue?>.MakeFailure(error);

    public ResultDto<TValue> BuildSuccess<TValue>(TValue value) => ResultDto<TValue>.MakeSuccess(value);
}
