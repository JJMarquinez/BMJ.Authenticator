using BMJ.Authenticator.Domain.ValueObjects;

namespace BMJ.Authenticator.Domain.Entities.Users.Builders;

public interface IUserBuilder
{
    IUserNameBuilder WithId(string id);
}
