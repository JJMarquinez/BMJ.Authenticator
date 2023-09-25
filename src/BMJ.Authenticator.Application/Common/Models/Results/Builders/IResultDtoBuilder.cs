using BMJ.Authenticator.Application.Common.Models.Errors;

namespace BMJ.Authenticator.Application.Common.Models.Results.Builders;

public interface IResultDtoBuilder
{
    ResultDto BuildSuccess();

    IResultDtoErrorBuilder WithError(ErrorDto errorDto);
}

public interface IResultDtoErrorBuilder
{
    ResultDto Build();
}
