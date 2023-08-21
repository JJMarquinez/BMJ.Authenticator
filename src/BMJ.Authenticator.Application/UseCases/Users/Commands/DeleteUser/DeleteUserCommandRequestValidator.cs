﻿using BMJ.Authenticator.Application.Common.Abstractions;
using FluentValidation;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser;

public class DeleteUserCommandRequestValidator : AbstractValidator<DeleteUserCommandRequest>
{
    private readonly IIdentityAdapter _identityAdapter;

    public DeleteUserCommandRequestValidator(IIdentityAdapter identityService)
    {
        _identityAdapter = identityService;

        RuleFor(v => v.Id)
            .NotEmpty()
            .Must(_identityAdapter.IsUserIdAssigned).WithMessage("It doesn't exist any user with the Id sent.");
    }
}
