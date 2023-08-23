using BMJ.Authenticator.Application.Common.Abstractions;
using FluentValidation;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    private readonly IIdentityAdapter _identityAdapter;

    public GetUserByIdQueryValidator(IIdentityAdapter identityService)
    {
        _identityAdapter = identityService;
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("The Id is mandatory to look for the user.")
            .Must(_identityAdapter.IsUserIdAssigned).WithMessage("It doesn't exist any user with the Id sent.");
    }
}
