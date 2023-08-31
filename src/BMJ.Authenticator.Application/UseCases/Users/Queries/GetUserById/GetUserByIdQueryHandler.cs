using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Users;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler
    : IRequestHandler<GetUserByIdQuery, ResultDto<UserDto?>>
{
    private readonly IIdentityAdapter _identityAdapter;

    public GetUserByIdQueryHandler(IIdentityAdapter identityAdapter)
    {
        _identityAdapter = identityAdapter;
    }

    public async Task<ResultDto<UserDto?>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        => await _identityAdapter.GetUserByIdAsync(request.Id!);
}
