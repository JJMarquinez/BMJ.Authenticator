using FluentValidation;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.LoginUser;

public class LoginUserCommandRequestValidator : AbstractValidator<LoginUserCommandRequest>
{
    public LoginUserCommandRequestValidator()
    {
        RuleFor(v => v.UserName).NotEmpty();
        RuleFor(v => v.Password).NotEmpty();
    }
}
