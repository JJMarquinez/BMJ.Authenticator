using BMJ.Authenticator.Application.Common.Interfaces;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Login.Commands
{
    public class LoginCommandHandler
        : IRequestHandler<LoginCommand, string>
    {
        private readonly IIdentityService _identityService;
        public LoginCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService; 
        }

        public async Task<string> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            string token = "";

            var user =  _identityService.GetUserAsync(command.UserName, command.Password);

            return token;
        }
    }
}
