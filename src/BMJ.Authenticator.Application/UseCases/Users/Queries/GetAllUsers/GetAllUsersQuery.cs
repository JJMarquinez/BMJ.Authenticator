using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Users;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers;

public record GetAllUsersQuery : IRequest<ResultDto<List<UserDto>?>>;
