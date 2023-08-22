using AutoMapper;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Domain.Entities.Users;
using BMJ.Authenticator.Domain.ValueObjects;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById;

public class GetUserByIdQueryRequestHandler
    : IRequestHandler<GetUserByIdQueryRequest, ResultDto<UserDto?>>
{
    private readonly IIdentityAdapter _identityAdapter;
    private readonly IMapper _mapper;

    public GetUserByIdQueryRequestHandler(IIdentityAdapter identityAdapter, IMapper mapper)
    {
        _identityAdapter = identityAdapter;
        _mapper = mapper;
    }

    public async Task<ResultDto<UserDto?>> Handle(GetUserByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var resultDto = await _identityAdapter.GetUserByIdAsync(request.Id!);

        if (resultDto.Success)
        {
            resultDto.Value = _mapper.Map<UserDto>(resultDto.Value!.ToUser());
        }
        return resultDto;
    }
}
