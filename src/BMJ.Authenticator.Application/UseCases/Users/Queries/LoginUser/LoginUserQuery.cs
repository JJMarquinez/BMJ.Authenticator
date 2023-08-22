using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.LoginUser;

/// <summary>
/// Authentication request
/// </summary>
public record LoginUserQuery() : IRequest<ResultDto<string?>>
{
    /// <summary>
    /// Username
    /// </summary>
    public string? UserName { get; init; }

    /// <summary>
    /// Password
    /// </summary>
    public string? Password { get; init; }
}
