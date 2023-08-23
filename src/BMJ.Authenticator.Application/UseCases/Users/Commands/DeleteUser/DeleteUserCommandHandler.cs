using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler
    : IRequestHandler<DeleteUserCommand, ResultDto>
{
    private readonly IIdentityAdapter _identityAdapter;

    public DeleteUserCommandHandler(IIdentityAdapter identityAdapter)
    {
        _identityAdapter = identityAdapter;
    }

    public async Task<ResultDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        => await _identityAdapter.DeleteUserAsync(request.Id!);
}
