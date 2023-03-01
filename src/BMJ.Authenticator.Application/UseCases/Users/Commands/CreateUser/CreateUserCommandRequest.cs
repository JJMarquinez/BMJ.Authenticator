using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser;

public record CreateUserCommandRequest : IRequest<ResultDto<string?>>
{
    public string? UserName { get; init; }
    public string? Password { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
}
