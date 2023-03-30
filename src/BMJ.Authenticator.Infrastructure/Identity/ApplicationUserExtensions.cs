using BMJ.Authenticator.Domain.Entities.Users;
using BMJ.Authenticator.Domain.ValueObjects;

namespace BMJ.Authenticator.Infrastructure.Identity
{
    public static class ApplicationUserExtensions
    {
        public static User ToApplicationUser(this ApplicationUser applicationUser, string[]? roles)
            => User.Builder()
            .WithId(applicationUser.Id)
            .WithName(applicationUser.UserName!)
            .WithEmail(Email.From(applicationUser.Email!))
            .WithRoles(roles!)
            .WithPhone(applicationUser.PhoneNumber is not null ? Phone.New(applicationUser.PhoneNumber) : null!)
            .WithPasswordHash(applicationUser.PasswordHash!)
            .Build();
    }
}
