namespace BMJ.Authenticator.Application.Common.Models.Errors.Builders;

public class ErrorDtoBuilder : IErrorDtoBuilder
{
    private string _code = null!;
    private string _title = null!;
    private string _detail = null!;
    private int _httpStatusCode = 0;

    public ErrorDto Build() => new()
    {
        Code = _code,
        Title = _title,
        Detail = _detail,
        HttpStatusCode = _httpStatusCode
    };

    public IErrorDtoBuilder WithCode(string code)
    {
        _code = code;
        return this;
    }

    public IErrorDtoBuilder WithDetail(string detail)
    {
        _detail = detail;
        return this;
    }

    public IErrorDtoBuilder WithHttpStatusCode(int httpStatusCode)
    {
        _httpStatusCode = httpStatusCode;
        return this;
    }

    public IErrorDtoBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }
}
