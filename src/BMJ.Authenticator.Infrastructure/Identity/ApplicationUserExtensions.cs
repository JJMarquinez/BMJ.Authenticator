namespace BMJ.Authenticator.Infrastructure.Identity
{
    public static class ApplicationUserExtensions
    {
        public static UserIdentification ToUserIdentification(this ApplicationUser applicationUser, string[]? roles)
            => UserIdentification.Builder()
            .WithId(applicationUser.Id)
            .WithUserName(applicationUser.UserName!)
            .WithEmail(applicationUser.Email!)
            .WithRoles(roles)
            .WithPhoneNumber(applicationUser.PhoneNumber)
            .Build();
    }
}
