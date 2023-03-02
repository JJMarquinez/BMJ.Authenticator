using FluentValidation;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserByName;

public class GetUserByNameQueryRequestValidator : AbstractValidator<GetUserByIdQueryRequest>
{
    public GetUserByNameQueryRequestValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("The Id is mandatory to look for the user.");
    }
}
