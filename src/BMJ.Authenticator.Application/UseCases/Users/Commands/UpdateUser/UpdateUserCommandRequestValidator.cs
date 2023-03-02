using BMJ.Authenticator.Application.Common.Interfaces;
using BMJ.Authenticator.Application.UseCases.Users.Commands.Common;
using FluentValidation;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser;

public class UpdateUserCommandRequestValidator : UserCommandRequestValidator<UpdateUserCommandRequest>
{
    private readonly IIdentityService _identityService;

    public UpdateUserCommandRequestValidator(IIdentityService identityService) : base(identityService)
    {
        _identityService = identityService;

        RuleFor(v => v.Id)
            .NotEmpty()
            .Must(DoesIdExist).WithMessage("It doesn't exist any user with the Id sent.");
    }

    public bool DoesIdExist(string userName)
    {
        return _identityService.IsUserIdAssigned(userName);
    }
}
