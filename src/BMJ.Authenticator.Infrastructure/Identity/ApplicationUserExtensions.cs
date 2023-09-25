using BMJ.Authenticator.Infrastructure.Identity.Builders;

namespace BMJ.Authenticator.Infrastructure.Identity
{
    public static class ApplicationUserExtensions
    {
        public static UserIdentification ToUserIdentification(this ApplicationUser applicationUser, string[]? roles)
            => new UserIdentificationBuilder()
            .WithId(applicationUser.Id)
            .WithUserName(applicationUser.UserName!)
            .WithEmail(applicationUser.Email!)
            .WithRoles(roles!)
            .WithPhoneNumber(applicationUser.PhoneNumber!)
            .Build();
    }
}
