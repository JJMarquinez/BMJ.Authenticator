using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Users;
using System.Text.Json;

namespace BMJ.Authenticator.Adapter.Identity;

public class IdentityAdapter : IIdentityAdapter
{
    private readonly IIdentityService _identityService;
    private readonly IAuthLogger _logger;

    public IdentityAdapter(IIdentityService identityService, IAuthLogger logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<ResultDto<UserDto?>> AuthenticateMemberAsync(string userName, string password)
    {
        var result = await _identityService.AuthenticateMemberAsync(userName, password);
        return result.Success
            ? ResultDto<UserDto?>.NewSuccess(JsonSerializer.Deserialize<UserDto>(result.Value!))
            : ResultDto<UserDto?>.NewFailure<UserDto?>(result.Error);
    }

    public async Task<ResultDto> CreateUserAsync(string userName, string password, string email, string? phoneNumber)
    {
        var resultDto = await _identityService.CreateUserAsync(userName, password, email, phoneNumber);

        if(!resultDto.Success) 
            _logger.Error("The user was not created due to the following error: @Error", resultDto.Error);

        return resultDto;
    }

    public async Task<ResultDto> DeleteUserAsync(string userId)
    {
        var resultDto = await _identityService.DeleteUserAsync(userId);

        if (!resultDto.Success)
            _logger.Error("The user was not deleted due to the following error: @Error", resultDto.Error);

        return resultDto;
    }

    public bool DoesUserNameNotExist(string userName)
        => _identityService.DoesUserNameNotExist(userName);

    public async Task<ResultDto<List<UserDto>?>> GetAllUserAsync()
    {
        var result = await _identityService.GetAllUserAsync();
        return result.Success
            ? ResultDto<List<UserDto>>.NewSuccess(JsonSerializer.Deserialize<List<UserDto>?>(result.Value!))
            : ResultDto<List<UserDto>>.NewFailure<List<UserDto>?>(result.Error);
    }

    public async Task<ResultDto<UserDto?>> GetUserByIdAsync(string userId)
    {
        var result = await _identityService.GetUserByIdAsync(userId);
        return result.Success
            ? ResultDto<UserDto?>.NewSuccess(JsonSerializer.Deserialize<UserDto?>(result.Value!))
            : ResultDto<UserDto?>.NewFailure<UserDto?>(result.Error);
    }

    public bool IsUserIdAssigned(string userId)
        => _identityService.IsUserIdAssigned(userId);

    public async Task<ResultDto> UpdateUserAsync(string userId, string userName, string email, string? phoneNumber)
    {
        var resultDto = await _identityService.UpdateUserAsync(userId, userName, email, phoneNumber);

        if (!resultDto.Success)
            _logger.Error("The user was not updated due to the following error: @Error", resultDto.Error);

        return resultDto;
    }
}
