using BMJ.Authenticator.Infrastructure.Identity;

namespace BMJ.Authenticator.Tool.Identity.UserOperators;

public interface IUserOperator : IDisposable
{
    ValueTask<string?> AddAsync(ApplicationUser applicationUser, string password, string[] roles);

    ValueTask<ApplicationUser?> FindAsync(string applicationUserId);
}
