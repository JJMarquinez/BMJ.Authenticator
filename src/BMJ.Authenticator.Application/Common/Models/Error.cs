using BMJ.Authenticator.Domain;
using System.Net;

namespace BMJ.Authenticator.Application.Common.Models;

public enum ErrorType
{
    Thechnical = 1,
    Functional = 2
}

public class Error
{
    private string code;
    private IEnumerable<string> descriptions;
    private ErrorType type;
    private HttpStatusCode statusCode;

    private Error(string code, IEnumerable<string> descriptions, ErrorType type, HttpStatusCode statusCode)
    {
        Ensure.Argument.NotNullOrEmpty(code, nameof(code));
        Ensure.Argument.Is(descriptions != null && descriptions.Count() > 0, nameof(descriptions));
        Ensure.Argument.IsNot(statusCode == 0, nameof(statusCode));
        this.code = code;
        this.descriptions = descriptions;
        this.type = type;
        this.statusCode = statusCode;
    }

    public static Error New(string code, IEnumerable<string> descriptions, ErrorType type, HttpStatusCode statusCode)
        => new(code, descriptions, type, statusCode);

    public string GetCode() => code;
    public IEnumerable<string> GetDescriptions() => descriptions;
    public ErrorType GetErrorType() => type;
    public HttpStatusCode GetStatusCode() => statusCode;
    public int GetStatusCodeAsInt() => (int)GetStatusCode();
}
