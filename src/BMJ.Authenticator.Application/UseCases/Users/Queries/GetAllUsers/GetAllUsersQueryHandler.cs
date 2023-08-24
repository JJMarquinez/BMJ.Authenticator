using AutoMapper;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Users;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler
    : IRequestHandler<GetAllUsersQuery, ResultDto<List<UserDto>?>>
{
    private readonly IIdentityAdapter _identityAdapter;

    public GetAllUsersQueryHandler(IIdentityAdapter identityAdapter)
    {
        _identityAdapter = identityAdapter;
    }

    public async Task<ResultDto<List<UserDto>?>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        => await _identityAdapter.GetAllUserAsync();
}
