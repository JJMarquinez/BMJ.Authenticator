using AutoMapper;
using BMJ.Authenticator.Application.Common.Interfaces;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Domain.Common.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser;

public class UpdateUserCommandRequestHandler
    : IRequestHandler<UpdateUserCommandRequest, ResultDto>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public UpdateUserCommandRequestHandler(IIdentityService identityService, IMapper mapper)
    {
        _identityService = identityService;
        _mapper = mapper;
    }

    public async Task<ResultDto> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
    {
        Result userResult = await _identityService.UpdateUserAsync(request.Id, request.UserName, request.Email, request.PhoneNumber);
        return _mapper.Map<ResultDto>(userResult);
    }
}
