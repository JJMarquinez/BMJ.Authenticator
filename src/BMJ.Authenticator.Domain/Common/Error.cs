namespace BMJ.Authenticator.Domain.Common;

public class Error
{
    private string _code;
    private string _title;
    private string _detail;
    private int _httpStatusCode;

    private Error(string code, string title, string detail, int httpStatusCode)
    {
        Ensure.Argument.NotNullOrEmpty(code, nameof(code));
        Ensure.Argument.NotNullOrEmpty(title, nameof(title));
        Ensure.Argument.NotNullOrEmpty(detail, nameof(detail));
        Ensure.Argument.IsNot(httpStatusCode == default, nameof(httpStatusCode));
        _code = code;
        _title = title;
        _detail = detail;
        _httpStatusCode = httpStatusCode;
    }

    public string GetCode() => _code;

    public string GetTitle() => _title;
    public string GetDetail() => _detail;

    public int GetHttpStatusCode => _httpStatusCode;

    public static Error New(string code, string message, string detail, int httpStatusCode) => new(code, message, detail, httpStatusCode);

    public static implicit operator string(Error error) => error.GetCode();

    public static Error None = new("None", "None", "None", 200);
}
