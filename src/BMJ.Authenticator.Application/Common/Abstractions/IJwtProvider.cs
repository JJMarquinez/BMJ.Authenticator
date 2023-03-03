
using BMJ.Authenticator.Domain.Entities.Users;

namespace BMJ.Authenticator.Application.Common.Abstractions
{
    public interface IJwtProvider
    {
        string Generate(User user);
    }
}
