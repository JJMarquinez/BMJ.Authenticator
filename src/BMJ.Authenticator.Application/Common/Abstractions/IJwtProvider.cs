using BMJ.Authenticator.Application.Common.Models.Users;

namespace BMJ.Authenticator.Application.Common.Abstractions
{
    public interface IJwtProvider
    {
        string Generate(UserDto user);
    }
}
