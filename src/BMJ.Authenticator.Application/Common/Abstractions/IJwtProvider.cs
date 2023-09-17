using BMJ.Authenticator.Application.Common.Models.Users;

namespace BMJ.Authenticator.Application.Common.Abstractions
{
    public interface IJwtProvider
    {
        ValueTask<string> GenerateAsync(UserDto user);
    }
}
