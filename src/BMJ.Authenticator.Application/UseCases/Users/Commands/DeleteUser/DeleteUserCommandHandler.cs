using AutoMapper;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler
    : IRequestHandler<DeleteUserCommand, ResultDto>
{
    private readonly IIdentityAdapter _identityAdapter;
    private readonly IMapper _mapper;

    public DeleteUserCommandHandler(IIdentityAdapter identityService, IMapper mapper)
    {
        _identityAdapter = identityService;
        _mapper = mapper;
    }

    public async Task<ResultDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        => await _identityAdapter.DeleteUserAsync(request.Id!);
}
