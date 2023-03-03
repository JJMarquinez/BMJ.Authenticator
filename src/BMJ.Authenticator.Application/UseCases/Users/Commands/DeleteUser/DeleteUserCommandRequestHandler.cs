using AutoMapper;
using BMJ.Authenticator.Application.Common.Interfaces;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser;
using BMJ.Authenticator.Domain.Common.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser;

public class DeleteUserCommandRequestHandler
    : IRequestHandler<DeleteUserCommandRequest, ResultDto>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public DeleteUserCommandRequestHandler(IIdentityService identityService, IMapper mapper)
    {
        _identityService = identityService;
        _mapper = mapper;
    }

    public async Task<ResultDto> Handle(DeleteUserCommandRequest request, CancellationToken cancellationToken)
    {
        Result userResult = await _identityService.DeleteUserAsync(request.Id);
        return _mapper.Map<ResultDto>(userResult);
    }
}
