using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Users;

namespace BMJ.Authenticator.Application.UnitTests.DependencyInjection;

public class IdentityAdapterTest : IIdentityAdapter
{
    public Task<ResultDto<UserDto?>> AuthenticateMemberAsync(string userName, string password)
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

    public Task<ResultDto<List<UserDto>?>> GetAllUserAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ResultDto<UserDto?>> GetUserByIdAsync(string userName)
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
