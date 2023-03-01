using BMJ.Authenticator.Domain.Common.Results;
using BMJ.Authenticator.Domain.Entities.Users;

namespace BMJ.Authenticator.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Result<List<User>?>> GetAllUserAsync();

        Task<Result<User?>> GetUserByNameAsync(string userName);

        Task<Result<User?>> AuthenticateMemberAsync(string userName, string password);

        Task<Result<bool>> IsInRoleAsync(string userId, string role);

        Task<Result<string?>> CreateUserAsync(string userName, string password, string email, string? phoneNumber);

        Task<Result> DeleteUserAsync(string userId);
    }
}
