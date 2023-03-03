using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById;

public record GetUserByIdQueryRequest : IRequest<ResultDto<UserDto?>>
{
    public string? Id { get; init; }
}
