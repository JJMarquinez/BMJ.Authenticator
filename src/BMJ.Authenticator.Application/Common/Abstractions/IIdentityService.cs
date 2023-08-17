using BMJ.Authenticator.Domain.Common.Results;
using BMJ.Authenticator.Domain.Entities.Users;

namespace BMJ.Authenticator.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Result<List<User>?>> GetAllUserAsync();

        Task<Result<User?>> GetUserByIdAsync(string userName);

        Task<Result<User?>> AuthenticateMemberAsync(string userName, string password);

        Task<Result<string?>> CreateUserAsync(string userName, string password, string email, string? phoneNumber);

        Task<Result> UpdateUserAsync(string userId, string userName, string email, string? phoneNumber);

        Task<Result> DeleteUserAsync(string userId);

        bool DoesUserNameNotExist(string userName);

        bool IsUserIdAssigned(string userId);
    }
}
