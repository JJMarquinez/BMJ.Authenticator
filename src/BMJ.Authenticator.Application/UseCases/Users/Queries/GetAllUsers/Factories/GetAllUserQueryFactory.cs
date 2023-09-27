using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Users;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers.Factories;

public class GetAllUserQueryFactory : IGetAllUserQueryFactory
{
    public IRequest<ResultDto<List<UserDto>?>> Genarate() => new GetAllUsersQuery();
}
