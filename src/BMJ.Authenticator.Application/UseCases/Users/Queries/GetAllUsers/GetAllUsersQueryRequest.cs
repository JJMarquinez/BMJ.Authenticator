using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers;

public record GetAllUsersQueryRequest : IRequest<ResultDto<List<UserDto>?>>;
