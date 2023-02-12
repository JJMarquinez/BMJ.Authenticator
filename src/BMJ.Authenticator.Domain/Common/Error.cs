namespace BMJ.Authenticator.Domain.Common;

public class Error : IEquatable<Error>
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

    public static bool operator ==(Error a, Error b)
    {
        bool result = false;
        if (a is null && b is null)
            result = true;
        else if (a is null ^ b is null)
            result = false;
        else if (a is not null && b is not null)
        {
            if (string.Equals(a._code, b._code, StringComparison.Ordinal)
                && string.Equals(a._message, b._message, StringComparison.InvariantCulture))
                result = true;
            else
                result = false;
        }
        return result;
    }

    public static bool operator !=(Error a, Error b) => !(a == b);

    public override bool Equals(object obj)
        => !this.GetType().Equals(obj.GetType()) ? false : this.Equals(obj as Error);

    public override int GetHashCode() => _code.GetHashCode() + _message.GetHashCode();

    public bool Equals(Error? other) =>  other is null ? false : this == other;
}
