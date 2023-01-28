using FluentValidation;

namespace BMJ.Authenticator.Application.UseCases.Login.Commands;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(v => v.UserName).NotEmpty();
        RuleFor(v => v.Password).NotEmpty();
    }
}
