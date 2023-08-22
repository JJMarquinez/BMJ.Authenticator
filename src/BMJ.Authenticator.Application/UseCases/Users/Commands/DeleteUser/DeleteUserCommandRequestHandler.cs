using AutoMapper;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser;

public class DeleteUserCommandRequestHandler
    : IRequestHandler<DeleteUserCommandRequest, ResultDto>
{
    private readonly IIdentityAdapter _identityAdapter;
    private readonly IMapper _mapper;

    public DeleteUserCommandRequestHandler(IIdentityAdapter identityService, IMapper mapper)
    {
        _identityAdapter = identityService;
        _mapper = mapper;
    }

    public async Task<ResultDto> Handle(DeleteUserCommandRequest request, CancellationToken cancellationToken)
        => await _identityAdapter.DeleteUserAsync(request.Id!);
}
