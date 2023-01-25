using BMJ.Authenticator.Application.Common.Models;

namespace BMJ.Authenticator.Infrastructure.Identity
{
    public static class ApplicationUserExtensions
    {
        public static User ToApplicationUser(this ApplicationUser userApplication, string[] roles)
            => User.New(userApplication.Id, userApplication.UserName, roles);
    }
}
