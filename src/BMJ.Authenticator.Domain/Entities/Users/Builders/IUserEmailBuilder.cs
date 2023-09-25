using BMJ.Authenticator.Domain.ValueObjects;

namespace BMJ.Authenticator.Domain.Entities.Users.Builders;

public interface IUserEmailBuilder
{
    IUserRolesPhonePasswordBuilder WithEmail(Email email);
}
