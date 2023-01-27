﻿using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Login.Commands
{
    public record LoginCommand() : IRequest<string>
    {
        public string? UserName { get; init; }

        public string? Password { get; init; }
    }
}
