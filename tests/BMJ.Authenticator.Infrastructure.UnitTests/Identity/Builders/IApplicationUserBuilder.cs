using BMJ.Authenticator.Infrastructure.Identity;

namespace BMJ.Authenticator.Infrastructure.UnitTests.Identity.Builders;

internal interface IApplicationUserBuilder
{
    IApplicationUserBuilder WithId(string id);
    IApplicationUserBuilder WithUserName(string userName);
    IApplicationUserBuilder WithEmail(string email);
    IApplicationUserBuilder WithPhoneNumber(string phoneNumber);
    IApplicationUserBuilder WithPasswordHash(string passwordHash);
    ApplicationUser Build();
}
