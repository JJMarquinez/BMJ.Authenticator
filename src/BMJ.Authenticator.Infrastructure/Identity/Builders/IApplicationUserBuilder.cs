namespace BMJ.Authenticator.Infrastructure.Identity.Builders;

public interface IApplicationUserBuilder
{
    IApplicationUserBuilder WithId(string id);
    IApplicationUserBuilder WithUserName(string userName);
    IApplicationUserBuilder WithEmail(string email);
    IApplicationUserBuilder WithPhoneNumber(string? phoneNumber);
    IApplicationUserBuilder WithPasswordHash(string passwordHash);
    ApplicationUser Build();
}
