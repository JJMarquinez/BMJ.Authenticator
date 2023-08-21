using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Domain.Common.Results;

namespace BMJ.Authenticator.Adapter.Common.Abstractions;

public interface IIdentityService
{
    Task<ResultDto<string?>> GetAllUserAsync();

    Task<ResultDto<string?>> GetUserByIdAsync(string userId);

    Task<ResultDto<string?>> AuthenticateMemberAsync(string userName, string password);

    Task<ResultDto<string?>> CreateUserAsync(string userName, string password, string email, string? phoneNumber);

    Task<ResultDto> UpdateUserAsync(string userId, string userName, string email, string? phoneNumber);

    Task<ResultDto> DeleteUserAsync(string userId);

    bool DoesUserNameNotExist(string userName);

    bool IsUserIdAssigned(string userId);
}
