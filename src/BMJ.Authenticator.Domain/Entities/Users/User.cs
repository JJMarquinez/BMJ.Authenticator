using BMJ.Authenticator.Domain.Common;
using BMJ.Authenticator.Domain.ValueObjects;

namespace BMJ.Authenticator.Domain.Entities.Users;

public class User
{
    private string _id;
    private string _userName;
    private Email _email;
    private Phone? _phoneNumber;
    private string _passwordHash;
    private string[] _roles;

    private User(string id, string userName, Email email, string[]? roles, Phone? phoneNumber, string passwordHash)
    {
        Ensure.Argument.NotNullOrEmpty(id, string.Format("{0} cannot be null or empty.", nameof(id)));
        Ensure.Argument.NotNullOrEmpty(userName, string.Format("{0} cannot be null or empty.", nameof(userName)));
        Ensure.Argument.NotNull(email, string.Format("{0} cannot be null or empty.", nameof(email)));

        _id = id;
        _userName = userName;
        _email = email;
        _phoneNumber = phoneNumber;
        _passwordHash = passwordHash;
        _roles = roles;
    }

    public static User New(string id, string userName, Email email, string[]? roles, Phone? phoneNumber, string passwordHash)
        => new(id, userName, email, roles, phoneNumber, passwordHash);

    public string GetId() => _id;
    public string GetUserName() => _userName;
    public Email GetEmail() => _email;
    public string[] GetRoles() => _roles;
    public Phone? GetPhoneNumber() => _phoneNumber;
    public string GetPasswordHash() => _passwordHash;
}
