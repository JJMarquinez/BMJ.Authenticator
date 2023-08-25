using BMJ.Authenticator.Domain.Common;
using BMJ.Authenticator.Domain.Entities.Users.Builders;
using BMJ.Authenticator.Domain.ValueObjects;

namespace BMJ.Authenticator.Domain.Entities.Users;

public class User
{
    public string Id { get; }
    public string UserName { get; }
    public Email Email { get; }
    public Phone? PhoneNumber { get; }
    public string[]? Roles { get; }

    private User(string id, string userName, Email email, string[]? roles, Phone? phoneNumber)
    {
        Ensure.Argument.NotNullOrEmpty(id, string.Format("{0} cannot be null or empty.", nameof(id)));
        Ensure.Argument.NotNullOrEmpty(userName, string.Format("{0} cannot be null or empty.", nameof(userName)));
        Ensure.Argument.NotNull(email, string.Format("{0} cannot be null or empty.", nameof(email)));

        Id = id;
        UserName = userName;
        Email = email;
        PhoneNumber = phoneNumber;
        Roles = roles;
    }

    internal static User NewInstance(string id, string userName, Email email, string[]? roles, Phone? phoneNumber)
        => new(id, userName, email, roles, phoneNumber);

    public static IUserBuilder Builder() => UserBuilder.NewInstance();
}
