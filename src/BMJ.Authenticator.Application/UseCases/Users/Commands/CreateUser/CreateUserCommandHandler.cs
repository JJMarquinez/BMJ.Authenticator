using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser;

public class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand, ResultDto>
{
    private readonly IIdentityAdapter _identityAdapter;

    public CreateUserCommandHandler(IIdentityAdapter identityAdapter)
    {
        _identityAdapter = identityAdapter;
    }

    public async Task<ResultDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        => await _identityAdapter.CreateUserAsync(request.UserName!, request.Password!, request.Email!, request.PhoneNumber);
}
