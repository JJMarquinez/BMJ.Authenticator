using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Login.Commands
{
    public record LoginCommand() : IRequest<ResultDto<string?>>
    {
        public string? UserName { get; init; }

        public string? Password { get; init; }
    }
}
