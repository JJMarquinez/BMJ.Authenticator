using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Users;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById;

public record GetUserByIdQuery : IRequest<ResultDto<UserDto?>>
{
    public string? Id { get; init; }
}
