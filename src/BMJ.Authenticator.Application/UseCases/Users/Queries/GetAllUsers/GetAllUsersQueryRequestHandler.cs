using AutoMapper;
using BMJ.Authenticator.Application.Common.Interfaces;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Domain.Common.Results;
using BMJ.Authenticator.Domain.Entities.Users;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers;

public class GetAllUsersQueryRequestHandler
    : IRequestHandler<GetAllUsersQueryRequest, ResultDto<List<UserDto>?>>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public GetAllUsersQueryRequestHandler(IIdentityService identityService, IMapper mapper)
    {
        _identityService = identityService;
        _mapper = mapper;
    }

    public async Task<ResultDto<List<UserDto>?>> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
    {
        Result<List<User>?> allUsersResult = await _identityService.GetAllUserAsync();

        return _mapper.Map<ResultDto<List<UserDto>?>>(allUsersResult);
    }
}
