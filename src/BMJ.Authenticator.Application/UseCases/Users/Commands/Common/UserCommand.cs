namespace BMJ.Authenticator.Application.UseCases.Users.Commands.Common;

public record UserCommand
{
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
}
