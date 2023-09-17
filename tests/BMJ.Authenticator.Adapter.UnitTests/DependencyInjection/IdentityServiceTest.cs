using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;

namespace BMJ.Authenticator.Adapter.UnitTests.DependencyInjection;

public class IdentityServiceTest : IIdentityService
{
    public Task<ResultDto<string?>> AuthenticateMemberAsync(string userName, string password)
    {
        throw new NotImplementedException();
    }

    public Task<ResultDto> CreateUserAsync(string userName, string password, string email, string? phoneNumber)
    {
        throw new NotImplementedException();
    }

    public Task<ResultDto> DeleteUserAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public bool DoesUserNameNotExist(string userName)
    {
        throw new NotImplementedException();
    }

    public Task<ResultDto<string?>> GetAllUserAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ResultDto<string?>> GetUserByIdAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public bool IsUserIdAssigned(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<ResultDto> UpdateUserAsync(string userId, string userName, string email, string? phoneNumber)
    {
        throw new NotImplementedException();
    }
}
