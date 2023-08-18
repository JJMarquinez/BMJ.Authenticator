using BMJ.Authenticator.Infrastructure.Identity.Builders;
using Microsoft.AspNetCore.Identity;

namespace BMJ.Authenticator.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public static IApplicationUserBuilder Builder() => ApplicationUserBuilder.New();
    }
}
