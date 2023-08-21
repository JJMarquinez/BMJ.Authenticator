using AutoMapper;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Domain.Entities.Users;
using BMJ.Authenticator.Domain.ValueObjects;
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
    {
        var resultDto = await _identityAdapter.GetUserByIdAsync(request.Id!);
        
        var userBuilder = User.Builder()
            .WithId(resultDto.Value!.Id)
            .WithName(resultDto.Value!.UserName)
            .WithEmail((Email)resultDto.Value!.Email)
            .WithRoles(resultDto.Value!.Roles);
        
        if (resultDto.Value!.PhoneNumber is not null)
            userBuilder.WithPhone((Phone)resultDto.Value!.PhoneNumber)
;
        var user = userBuilder.Build();
        resultDto.Value = _mapper.Map<UserDto>(user);
        return resultDto;
    }
}
