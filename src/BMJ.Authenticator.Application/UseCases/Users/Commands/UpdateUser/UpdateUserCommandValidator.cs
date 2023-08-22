using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.UseCases.Users.Commands.Common;
using FluentValidation;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : UserCommandValidator<UpdateUserCommand>
{
    private readonly IIdentityAdapter _identityAdapter;

    public UpdateUserCommandValidator(IIdentityAdapter identityService) : base(identityService)
    {
        _identityAdapter = identityService;

        RuleFor(v => v.Id)
            .NotEmpty()
            .Must(_identityAdapter.IsUserIdAssigned).WithMessage("It doesn't exist any user with the Id sent.");
    }
}
