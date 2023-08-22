using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.UseCases.Users.Commands.Common;
using FluentValidation;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser;

public class CreateUserCommandValidator : UserCommandValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(IIdentityAdapter identityService) 
        : base(identityService)
    {
        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Your password cannot be empty")
            .MinimumLength(8).WithMessage("Your password length must be at least 8.")
            .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
            .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");
    }
}
