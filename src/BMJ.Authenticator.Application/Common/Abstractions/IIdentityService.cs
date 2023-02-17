using BMJ.Authenticator.Application.Common.Models;
using BMJ.Authenticator.Domain.Common.Results;

namespace BMJ.Authenticator.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Result<string?>> GetUserNameAsync(string userId);

        Task<Result<User?>> AuthenticateMember(string userName, string password);

        Task<Result<bool>> IsInRoleAsync(string userId, string role);

        Task<Result> CreateUserAsync(string userName, string password);

        Task<Result> DeleteUserAsync(string userId);
    }
}
