using BMJ.Authenticator.Domain.Common;
using BMJ.Authenticator.Domain.ValueObjects;

namespace BMJ.Authenticator.Domain.Entities.Users;

public class User
{
    private string _id;
    private string _userName;
    private Email _email;
    private Phone _phoneNumber;
    private string _passwordHash;

    private User(string id, string userName, Email email, Phone phoneNumber, string passwordHash)
    {
        Ensure.Argument.NotNullOrEmpty(id, string.Format("{0} cannot be null or empty", nameof(id)));
        Ensure.Argument.NotNullOrEmpty(userName, string.Format("{0} cannot be null or empty", nameof(userName)));
        Ensure.Argument.NotNull(email, string.Format("{0} cannot be null or empty", nameof(email)));

        _id = id;
        _userName = userName;
        _email = email;
        _phoneNumber = phoneNumber;
        _passwordHash = passwordHash;
    }

    public static User New(string id, string userName, Email email, Phone phoneNumber, string passwordHash)
        => new(id, userName, email, phoneNumber, passwordHash);

    internal string GetId() => _id;
    internal string GetUserName() => _userName;
    internal Email GetEmail() => _email;
    internal Phone GetPhoneNumber() => _phoneNumber;
    internal string GetPasswordHash() => _passwordHash;
}
