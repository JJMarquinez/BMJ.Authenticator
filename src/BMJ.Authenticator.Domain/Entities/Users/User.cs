using BMJ.Authenticator.Domain.Common;

namespace BMJ.Authenticator.Domain.Entities.Users;

public class User
{
    private string _id;
    private string _userName;
    private string _email;
    private string _phoneNumber;
    private string _passwordHash;

    private User(string id, string userName, string email, string phoneNumber, string passwordHash)
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

    public static User New(string id, string userName, string email, string phoneNumber, string passwordHash)
        => new(id, userName, email, phoneNumber, passwordHash);

    internal string GetId() => _id;
    internal string GetUserName() => _userName;
    internal string GetEmail() => _email;
    internal string GetPhoneNumber() => _phoneNumber;
    internal string GetPasswordHash() => _passwordHash;
}
