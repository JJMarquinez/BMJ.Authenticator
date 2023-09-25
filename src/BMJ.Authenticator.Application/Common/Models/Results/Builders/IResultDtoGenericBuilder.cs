using BMJ.Authenticator.Application.Common.Models.Errors;

namespace BMJ.Authenticator.Application.Common.Models.Results.Builders;

public interface IResultDtoGenericBuilder
{
    ResultDto<TValue> BuildSuccess<TValue>(TValue value);
    ResultDto<TValue?> BuildFailure<TValue>(ErrorDto error);
}
