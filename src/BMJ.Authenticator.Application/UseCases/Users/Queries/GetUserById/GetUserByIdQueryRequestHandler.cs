using AutoMapper;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models;
using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById;

public class GetUserByIdQueryRequestHandler
    : IRequestHandler<GetUserByIdQueryRequest, ResultDto<UserDto?>>
{
    private readonly IIdentityAdapter _identityAdapter;
    private readonly IMapper _mapper;

    public GetUserByIdQueryRequestHandler(IIdentityAdapter identityService, IMapper mapper)
    {
        _identityAdapter = identityService;
        _mapper = mapper;
    }

    public async Task<ResultDto<UserDto?>> Handle(GetUserByIdQueryRequest request, CancellationToken cancellationToken)
        => await _identityAdapter.GetUserByIdAsync(request.Id!);
}
