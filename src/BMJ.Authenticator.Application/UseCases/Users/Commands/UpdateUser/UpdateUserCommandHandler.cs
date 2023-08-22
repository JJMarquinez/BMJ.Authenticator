using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler
    : IRequestHandler<UpdateUserCommand, ResultDto>
{
    private readonly IIdentityAdapter _identityAdapter;

    public UpdateUserCommandHandler(IIdentityAdapter identityAdapter)
    {
        _identityAdapter = identityAdapter;
    }

    public async Task<ResultDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        => await _identityAdapter.UpdateUserAsync(request.Id!, request.UserName!, request.Email!, request.PhoneNumber);
}
