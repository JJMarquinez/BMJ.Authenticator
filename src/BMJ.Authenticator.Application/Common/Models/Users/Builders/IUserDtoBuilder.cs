namespace BMJ.Authenticator.Application.Common.Models.Users.Builders;

public interface IUserDtoBuilder
{
    IUserDtoBuilder WithId(string id);
    IUserDtoBuilder WithName(string name);
    IUserDtoBuilder WithEmail(string email);
    IUserDtoBuilder WithPhone(string phone);
    IUserDtoBuilder WithRoles(string[] roles);
    UserDto Build();
}
