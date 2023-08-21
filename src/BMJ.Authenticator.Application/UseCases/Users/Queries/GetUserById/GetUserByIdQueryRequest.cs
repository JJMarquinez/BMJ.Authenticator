using BMJ.Authenticator.Application.Common.Models;
using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById;

public record GetUserByIdQueryRequest : IRequest<ResultDto<UserDto?>>
{
    public string? Id { get; init; }
}
