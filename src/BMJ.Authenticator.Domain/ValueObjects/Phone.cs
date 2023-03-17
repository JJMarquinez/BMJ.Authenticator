using BMJ.Authenticator.Domain.Common;
using System.Text.RegularExpressions;

namespace BMJ.Authenticator.Domain.ValueObjects;

public class Phone : ValueObject
{
    internal string Number { get; private set; }

    static Phone() { }

    private Phone() { }

    private Phone(string number)
    {
        Ensure.Argument.NotNullOrEmpty(number, string.Format("{0} cannot be null or empty.", nameof(number)));
        Ensure.Argument.Is(IsValidPhoneNumber(number), string.Format("Invalid phone number ({0}).", nameof(number)));
        Number = number;
    }

    public static Phone New(string number)
    {
        return new(number);
    }

    private bool IsValidPhoneNumber(string number)
    {
        return Regex.IsMatch(number, @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
    }

    public override string ToString() => Number;

    public static explicit operator Phone(string number) => New(number);

    public static implicit operator string(Phone phone) => phone.ToString();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
    }
}
