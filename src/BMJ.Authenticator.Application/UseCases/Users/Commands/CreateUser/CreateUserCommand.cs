using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.UseCases.Users.Commands.Common;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser;

public record CreateUserCommand : UserCommand, IRequest<ResultDto>
{ 
    public string? Password { get; init; }
}
