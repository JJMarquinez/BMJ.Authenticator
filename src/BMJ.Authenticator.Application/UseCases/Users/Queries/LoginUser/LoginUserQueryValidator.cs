using FluentValidation;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.LoginUser;

public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
{
    public LoginUserQueryValidator()
    {
        RuleFor(v => v.UserName).NotEmpty();
        RuleFor(v => v.Password).NotEmpty();
    }
}
