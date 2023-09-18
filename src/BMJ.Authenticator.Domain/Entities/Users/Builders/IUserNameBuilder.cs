namespace BMJ.Authenticator.Domain.Entities.Users.Builders;

public interface IUserNameBuilder
{
    IUserEmailBuilder WithName(string name);
}
