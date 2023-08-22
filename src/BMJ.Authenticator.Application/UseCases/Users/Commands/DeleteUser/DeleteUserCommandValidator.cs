using BMJ.Authenticator.Application.Common.Abstractions;
using FluentValidation;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    private readonly IIdentityAdapter _identityAdapter;

    public DeleteUserCommandValidator(IIdentityAdapter identityService)
    {
        _identityAdapter = identityService;

        RuleFor(v => v.Id)
            .NotEmpty()
            .Must(_identityAdapter.IsUserIdAssigned).WithMessage("It doesn't exist any user with the Id sent.");
    }
}
