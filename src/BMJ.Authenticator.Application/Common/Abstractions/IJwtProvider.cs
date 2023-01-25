using BMJ.Authenticator.Application.Common.Models;

namespace BMJ.Authenticator.Application.Common.Abstractions
{
    public interface IJwtProvider
    {
        string Generate(User user);
    }
}
