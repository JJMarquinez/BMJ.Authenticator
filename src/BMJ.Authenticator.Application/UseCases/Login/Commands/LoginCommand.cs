using BMJ.Authenticator.Domain.Common;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Login.Commands
{
    public record LoginCommand() : IRequest<Result<string?>>
    {
        public string? UserName { get; init; }

        public string? Password { get; init; }
    }
}
