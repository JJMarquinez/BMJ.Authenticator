using BMJ.Authenticator.Domain.Entities.Users.Builders;

namespace BMJ.Authenticator.Infrastructure.Identity.Builders;

public interface IUserIdentificationBuilder
{
    IUserIdentificationBuilder WithId(string id);
    IUserIdentificationBuilder WithUserName(string userName);
    IUserIdentificationBuilder WithEmail(string email);
    IUserIdentificationBuilder WithPhoneNumber(string? phoneNumber);
    IUserIdentificationBuilder WithRoles(string[]? roles);
    UserIdentification Build();
}
