using FluentValidation;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserByName;

public class GetUserByNameQueryRequestValidator : AbstractValidator<GetUserByNameQueryRequest>
{
    public GetUserByNameQueryRequestValidator()
    {
        RuleFor(v => v.UserName).NotEmpty();
    }
}
