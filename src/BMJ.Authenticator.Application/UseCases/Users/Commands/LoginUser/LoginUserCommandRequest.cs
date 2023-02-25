using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.LoginUser
{
    public record LoginUserCommandRequest() : IRequest<ResultDto<string?>>
    {
        public string? UserName { get; init; }

        public string? Password { get; init; }
    }
}
