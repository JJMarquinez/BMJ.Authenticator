using BMJ.Authenticator.Application.Common.Interfaces;
using FluentValidation;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.Common;

public class UserCommandRequestValidator<T> 
    : AbstractValidator<T>
    where T : UserCommandRequest
{
    private readonly IIdentityService _identityService;

    public UserCommandRequestValidator(IIdentityService identityService)
    {
        _identityService = identityService;

        RuleFor(v => v.UserName)
            .NotEmpty()
            .Must(BeUniqueUserName).WithMessage("The specified user name already exists.");

        RuleFor(v => v.Email).NotEmpty().EmailAddress();

        RuleFor(v => v.PhoneNumber)
            .Matches(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")
            .WithMessage("Your phone number must have the following formats: 111-111-1111, 111.111.1111 or 111 111 1111.");
    }
    public bool BeUniqueUserName(string userName)
    {
        return _identityService.DoesUserNameNotExist(userName);
    }
}
