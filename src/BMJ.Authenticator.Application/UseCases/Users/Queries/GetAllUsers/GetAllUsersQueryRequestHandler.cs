using AutoMapper;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models;
using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers;

public class GetAllUsersQueryRequestHandler
    : IRequestHandler<GetAllUsersQueryRequest, ResultDto<List<UserDto>?>>
{
    private readonly IIdentityAdapter _identityAdapter;
    private readonly IMapper _mapper;

    public GetAllUsersQueryRequestHandler(IIdentityAdapter identityService, IMapper mapper)
    {
        _identityAdapter = identityService;
        _mapper = mapper;
    }

    public async Task<ResultDto<List<UserDto>?>> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
        => await _identityAdapter.GetAllUserAsync();
}
