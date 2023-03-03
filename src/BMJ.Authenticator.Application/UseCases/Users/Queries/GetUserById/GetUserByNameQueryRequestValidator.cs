using BMJ.Authenticator.Application.Common.Interfaces;
using FluentValidation;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById;

public class GetUserByNameQueryRequestValidator : AbstractValidator<GetUserByIdQueryRequest>
{
    private readonly IIdentityService _identityService;

    public GetUserByNameQueryRequestValidator(IIdentityService identityService)
    {
        _identityService = identityService;
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("The Id is mandatory to look for the user.")
            .Must(_identityService.IsUserIdAssigned).WithMessage("It doesn't exist any user with the Id sent."); ;
    }
}
