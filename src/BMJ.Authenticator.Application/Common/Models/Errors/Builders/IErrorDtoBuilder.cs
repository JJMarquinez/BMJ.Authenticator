namespace BMJ.Authenticator.Application.Common.Models.Errors.Builders;

public interface IErrorDtoBuilder
{
    IErrorDtoBuilder WithCode(string code);
    IErrorDtoBuilder WithTitle(string title);
    IErrorDtoBuilder WithDetail(string detail);
    IErrorDtoBuilder WithHttpStatusCode(int httpStatusCode);
    ErrorDto Build();

}
