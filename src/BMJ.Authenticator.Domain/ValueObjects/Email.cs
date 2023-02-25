using BMJ.Authenticator.Domain.Common;
using System.Net.Mail;

namespace BMJ.Authenticator.Domain.ValueObjects;

public class Email : ValueObject 
{
    internal string Address { get; private set; }

    static Email() { }

    private Email() { }

    private Email(string address)
    {
        Ensure.Argument.NotNullOrEmpty(address, string.Format("{0} cannot be null or empty.", nameof(address)));
        Ensure.Argument.Is(IsValidEmail(address), string.Format("Invalid email address ({0})."));
        Address = address;
    }

    public static Email From(string address)
    { 
        return new(address);
    }

    private static bool IsValidEmail(string address)
    {
        bool isValid = true;
        try
        {
            MailAddress addr = new MailAddress(address);
        }
        catch
        {
            isValid = false;
        }
        return isValid;
    }

    public override string ToString() => Address;

    public static explicit operator Email(string address) => From(address);

    public static implicit operator string(Email email) => email.ToString();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Address;
    }
}
