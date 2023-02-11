using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Interfaces;
using BMJ.Authenticator.Domain.Common;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Login.Commands;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, string>
{
    private readonly IIdentityService _identityService;
    private readonly IJwtProvider _jwtProvider;
    public LoginCommandHandler(IIdentityService identityService, IJwtProvider jwtProvider)
    {
        _identityService = identityService;
        _jwtProvider = jwtProvider; 
    }

    public async Task<string> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user =  await _identityService.AuthenticateMember(command.UserName, command.Password);
        Ensure.Not<UnauthorizedAccessException>(user is null);

        return _jwtProvider.Generate(user);
    }
}
