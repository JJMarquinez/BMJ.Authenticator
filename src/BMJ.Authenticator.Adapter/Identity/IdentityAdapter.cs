using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Results.Builders;
using BMJ.Authenticator.Application.Common.Models.Users;
using System.Text.Json;

namespace BMJ.Authenticator.Adapter.Identity;

public class IdentityAdapter : IIdentityAdapter
{
    private readonly IIdentityService _identityService;
    private readonly IResultDtoGenericBuilder _resultDtoGenericBuilder;

    public IdentityAdapter(IIdentityService identityService, IResultDtoGenericBuilder resultDtoGenericBuilder)
    {
        _identityService = identityService;
        _resultDtoGenericBuilder = resultDtoGenericBuilder;
    }

    public async Task<ResultDto<UserDto?>> AuthenticateMemberAsync(string userName, string password)
    {
        var result = await _identityService.AuthenticateMemberAsync(userName, password);
        return result.Success
            ? _resultDtoGenericBuilder.BuildSuccess(JsonSerializer.Deserialize<UserDto>(result.Value!))
            : _resultDtoGenericBuilder.BuildFailure<UserDto?>(result.Error);
    }

    public async Task<ResultDto> CreateUserAsync(string userName, string password, string email, string? phoneNumber)
        => await _identityService.CreateUserAsync(userName, password, email, phoneNumber);

    public async Task<ResultDto> DeleteUserAsync(string userId)
        => await _identityService.DeleteUserAsync(userId);


    public bool DoesUserNameNotExist(string userName)
        => _identityService.DoesUserNameNotExist(userName);

    public async Task<ResultDto<List<UserDto>?>> GetAllUserAsync()
    {
        var result = await _identityService.GetAllUserAsync();
        return result.Success
            ? _resultDtoGenericBuilder.BuildSuccess(JsonSerializer.Deserialize<List<UserDto>?>(result.Value!))
            : _resultDtoGenericBuilder.BuildFailure<List<UserDto>?>(result.Error);
    }

    public async Task<ResultDto<UserDto?>> GetUserByIdAsync(string userId)
    {
        var result = await _identityService.GetUserByIdAsync(userId);
        return result.Success
            ? _resultDtoGenericBuilder.BuildSuccess(JsonSerializer.Deserialize<UserDto?>(result.Value!)) 
            : _resultDtoGenericBuilder.BuildFailure<UserDto?>(result.Error);
    }

    public bool IsUserIdAssigned(string userId)
        => _identityService.IsUserIdAssigned(userId);

    public async Task<ResultDto> UpdateUserAsync(string userId, string userName, string email, string? phoneNumber)
        => await _identityService.UpdateUserAsync(userId, userName, email, phoneNumber);
}
