namespace BMJ.Authenticator.Domain.Common;

public class Error
{
    private string _code;
    private string _message;

    public Error(string code, string message)
    {
        Ensure.Argument.NotNullOrEmpty(code, nameof(code));
        Ensure.Argument.NotNullOrEmpty(message, nameof(message));
        _code = code;
        _message = message;
    }

    public string GetCode() => _code;
    public string GetMessage() => _message;
}
