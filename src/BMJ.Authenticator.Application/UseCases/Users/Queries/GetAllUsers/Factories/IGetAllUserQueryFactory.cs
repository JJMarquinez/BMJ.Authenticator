using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Users;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers.Factories;

public interface IGetAllUserQueryFactory
{
    IRequest<ResultDto<List<UserDto>?>> Genarate();
}
