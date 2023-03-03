using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser;

public class DeleteUserCommandRequest : IRequest<ResultDto>
{
    public string? Id { get; init; }
}
