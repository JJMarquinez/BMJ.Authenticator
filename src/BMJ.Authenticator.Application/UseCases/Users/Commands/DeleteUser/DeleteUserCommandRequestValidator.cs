using BMJ.Authenticator.Application.Common.Interfaces;
using FluentValidation;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser;

public class DeleteUserCommandRequestValidator : AbstractValidator<DeleteUserCommandRequest>
{
    private readonly IIdentityService _identityService;

    public DeleteUserCommandRequestValidator(IIdentityService identityService)
    {
        _identityService = identityService;

        RuleFor(v => v.Id)
            .NotEmpty()
            .Must(_identityService.IsUserIdAssigned).WithMessage("It doesn't exist any user with the Id sent.");
    }
}
