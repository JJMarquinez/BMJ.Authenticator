using AutoMapper;
using BMJ.Authenticator.Application.Common.Interfaces;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers;
using BMJ.Authenticator.Domain.Common.Results;
using BMJ.Authenticator.Domain.Entities.Users;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserByName;

public class GetUserByNameQueryRequestHandler
    : IRequestHandler<GetUserByNameQueryRequest, ResultDto<UserDto?>>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public GetUserByNameQueryRequestHandler(IIdentityService identityService, IMapper mapper)
    {
        _identityService = identityService;
        _mapper = mapper;
    }

    public async Task<ResultDto<UserDto?>> Handle(GetUserByNameQueryRequest request, CancellationToken cancellationToken)
    {
        Result<User?> allUsersResult = await _identityService.GetUserByNameAsync(request.UserName);
        return _mapper.Map<ResultDto<UserDto?>>(allUsersResult);
    }
}
