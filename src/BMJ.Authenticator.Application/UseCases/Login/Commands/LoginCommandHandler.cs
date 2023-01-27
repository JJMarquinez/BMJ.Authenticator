using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Interfaces;
using BMJ.Authenticator.Application.Common.Models;
using BMJ.Authenticator.Domain;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Login.Commands
{
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
            //Ensure.NotNull(user, "Invalid credentials");
            Ensure.Not<UnauthorizedAccessException>(user is null, "Invalid credentials");

            return _jwtProvider.Generate(user);
        }
    }
}
