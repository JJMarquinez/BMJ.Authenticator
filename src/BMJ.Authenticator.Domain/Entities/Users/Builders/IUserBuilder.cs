using BMJ.Authenticator.Domain.ValueObjects;

namespace BMJ.Authenticator.Domain.Entities.Users.Builders;

public interface IUserBuilder
{
    IUserNameBuilder WithId(string id);
}

public interface IUserNameBuilder
{
    IUserEmailBuilder WithName(string name);
}

public interface IUserEmailBuilder
{
    IUserRolesPhonePasswordBuilder WithEmail(Email email);
}

public interface IUserRolesPhonePasswordBuilder
{
    IUserRolesPhonePasswordBuilder WithRoles(string[] roles);
    IUserRolesPhonePasswordBuilder WithPhone(Phone phone);
    User Build();
}
