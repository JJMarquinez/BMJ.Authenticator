using BMJ.Authenticator.Domain.Common;
using BMJ.Authenticator.Domain.Entities.Users.Builders;
using BMJ.Authenticator.Domain.ValueObjects;

namespace BMJ.Authenticator.Domain.Entities.Users;

public class User
{
    private string _id;
    private string _userName;
    private Email _email;
    private Phone? _phone;
    private string[]? _roles;

    private User(string id, string userName, Email email, string[]? roles, Phone? phone)
    {
        Ensure.Argument.NotNullOrEmpty(id, string.Format("{0} cannot be null or empty.", nameof(id)));
        Ensure.Argument.NotNullOrEmpty(userName, string.Format("{0} cannot be null or empty.", nameof(userName)));
        Ensure.Argument.NotNull(email, string.Format("{0} cannot be null or empty.", nameof(email)));

        _id = id;
        _userName = userName;
        _email = email;
        _phone = phone;
        _roles = roles;
    }

    internal static User New(string id, string userName, Email email, string[]? roles, Phone? phone)
        => new(id, userName, email, roles, phone);

    public string GetId() => _id;
    public string GetUserName() => _userName;
    public Email GetEmail() => _email;
    public string[]? GetRoles() => _roles;
    public Phone? GetPhoneNumber() => _phone;

    public static IUserBuilder Builder() => UserBuilder.New();
}
