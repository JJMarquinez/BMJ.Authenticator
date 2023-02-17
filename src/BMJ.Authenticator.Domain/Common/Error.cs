namespace BMJ.Authenticator.Domain.Common;

public class Error
{
    private string _code;
    private string _message;

    private Error(string code, string message)
    {
        Ensure.Argument.NotNullOrEmpty(code, nameof(code));
        Ensure.Argument.NotNullOrEmpty(message, nameof(message));
        _code = code;
        _message = message;
    }

    public string GetCode() => _code;
    public string GetMessage() => _message;

    public static Error New(string code, string message) => new(code, message);

    public static implicit operator string(Error error) => error.GetCode();

    public static Error None = new("None", "None");
}
