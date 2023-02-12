using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Interfaces;
using BMJ.Authenticator.Application.Common.Models;
using BMJ.Authenticator.Domain.Common;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Login.Commands;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, Result<string?>>
{
    private readonly IIdentityService _identityService;
    private readonly IJwtProvider _jwtProvider;
    public LoginCommandHandler(IIdentityService identityService, IJwtProvider jwtProvider)
    {
        _identityService = identityService;
        _jwtProvider = jwtProvider; 
    }

    public async Task<Result<string?>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        Result<User?> userResult =  await _identityService.AuthenticateMember(command.UserName, command.Password);

        return userResult.IsFailure() 
            ? Result.Failure<string>(userResult.GetError())
            : _jwtProvider.Generate(userResult.Value);
    }
}
