using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models;
using BMJ.Authenticator.Application.Common.Models.Results;
using System.Text.Json;

namespace BMJ.Authenticator.Adapter.Identity;

public class IdentityAdapter : IIdentityAdapter
{
    private readonly IIdentityService _identityService;

    public IdentityAdapter(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ResultDto<UserDto?>> AuthenticateMemberAsync(string userName, string password)
    {
        var result = await _identityService.AuthenticateMemberAsync(userName, password);
        return result.Success
            ? ResultDto<UserDto?>.NewSuccess(JsonSerializer.Deserialize<UserDto>(result.Value!))
            : ResultDto<UserDto?>.NewFailure<UserDto?>(result.Error);
    }

    public async Task<ResultDto<string?>> CreateUserAsync(string userName, string password, string email, string? phoneNumber)
        => await _identityService.CreateUserAsync(userName,password,email,phoneNumber);

    public Task<ResultDto> DeleteUserAsync(string userId)
        => _identityService.DeleteUserAsync(userId);

    public bool DoesUserNameNotExist(string userName)
        => _identityService.DoesUserNameNotExist(userName);

    public async Task<ResultDto<List<UserDto>?>> GetAllUserAsync()
    {
        var result = await _identityService.GetAllUserAsync();
        return result.Success
            ? ResultDto<List<UserDto>>.NewSuccess(JsonSerializer.Deserialize<List<UserDto>?> (result.Value!))
            : ResultDto<List<UserDto>>.NewFailure<List<UserDto>?>(result.Error);
    }

    public async Task<ResultDto<UserDto?>> GetUserByIdAsync(string userId)
    {
        var result = await _identityService.GetUserByIdAsync(userId);
        return result.Success
            ? ResultDto<UserDto?>.NewSuccess<UserDto?>(JsonSerializer.Deserialize<UserDto?>(result.Value!))
            : ResultDto<UserDto?>.NewFailure<UserDto?>(result.Error);
    }

    public bool IsUserIdAssigned(string userId)
        => _identityService.IsUserIdAssigned(userId);

    public Task<ResultDto> UpdateUserAsync(string userId, string userName, string email, string? phoneNumber)
        => _identityService.UpdateUserAsync(userId, userName, email, phoneNumber);
}
