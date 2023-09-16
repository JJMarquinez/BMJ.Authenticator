using BMJ.Authenticator.Infrastructure.Identity;

namespace BMJ.Authenticator.ToolKit.Identity.UserOperators;

public interface IUserOperator : IDisposable
{
    ValueTask<string?> AddAsync(ApplicationUser applicationUser, string password, string[] roles);
}
