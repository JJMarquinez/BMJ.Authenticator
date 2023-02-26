using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserByName;

public record GetUserByNameQueryRequest : IRequest<ResultDto<UserDto?>>;
