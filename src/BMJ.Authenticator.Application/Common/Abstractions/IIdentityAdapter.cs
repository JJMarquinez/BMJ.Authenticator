using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Users;

namespace BMJ.Authenticator.Application.Common.Abstractions
{
    public interface IIdentityAdapter
    {
        Task<ResultDto<List<UserDto>?>> GetAllUserAsync();

        Task<ResultDto<UserDto?>> GetUserByIdAsync(string userName);

        Task<ResultDto<UserDto?>> AuthenticateMemberAsync(string userName, string password);

        Task<ResultDto> CreateUserAsync(string userName, string password, string email, string? phoneNumber);

        Task<ResultDto> UpdateUserAsync(string userId, string userName, string email, string? phoneNumber);

        Task<ResultDto> DeleteUserAsync(string userId);

        bool DoesUserNameNotExist(string userName);

        bool IsUserIdAssigned(string userId);
    }
}
