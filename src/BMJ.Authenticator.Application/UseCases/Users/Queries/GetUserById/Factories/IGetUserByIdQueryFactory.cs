using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Users;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById.Factories;

public interface IGetUserByIdQueryFactory
{
    IRequest<ResultDto<UserDto?>> Genarate(string id);
}
