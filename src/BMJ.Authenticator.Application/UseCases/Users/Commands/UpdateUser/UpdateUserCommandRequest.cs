using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.UseCases.Users.Commands.Common;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser;

public record UpdateUserCommandRequest : UserCommandRequest, IRequest<ResultDto>
{
    public string? Id { get; init; }
}
