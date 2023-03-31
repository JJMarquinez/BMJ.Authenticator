namespace BMJ.Authenticator.Domain.Common.Errors;

public class ErrorBuilder : IErrorBuilder, IErrorWithTitleBuilder, IErrorWithDetailBuilder, IErrorWithHttpStatusCodeBuilder, IErrorBuildBuilder
{
    private string _code = null!;
    private string _title = null!;
    private string _detail = null!;
    private int _httpStatusCode = 0;

    private ErrorBuilder() { }

    public static IErrorBuilder New() => new ErrorBuilder();

    public Error Build() => Error.New(_code, _title, _detail, _httpStatusCode);

    public IErrorWithTitleBuilder WithCode(string code)
    {
        _code = code;
        return this;
    }

    public IErrorWithHttpStatusCodeBuilder WithDetail(string detail)
    {
        _detail = detail;
        return this;
    }

    public IErrorWithDetailBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public IErrorBuildBuilder WithHttpStatusCode(int httpStatusCode)
    {
        _httpStatusCode = httpStatusCode;
        return this;
    }
}
