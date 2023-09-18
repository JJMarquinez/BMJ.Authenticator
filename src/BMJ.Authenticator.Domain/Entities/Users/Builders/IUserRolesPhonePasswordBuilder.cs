using BMJ.Authenticator.Domain.ValueObjects;

namespace BMJ.Authenticator.Domain.Entities.Users.Builders;

public interface IUserRolesPhonePasswordBuilder
{
    IUserRolesPhonePasswordBuilder WithRoles(string[] roles);
    IUserRolesPhonePasswordBuilder WithPhone(Phone phone);
    User Build();
}
