using BMJ.Authenticator.Api.Caching;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById;
using BMJ.Authenticator.Application.UseCases.Users.Queries.LoginUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace BMJ.Authenticator.Api.Controllers.v1.Members;

[Authorize]
[ApiVersion("1.0")]
public class MemberController : ApiControllerBase
{
    /// <summary>
    /// Reatrieves a user token to an authenticated user.
    /// </summary>
    /// <param name="loginCommandRequest">User information to get authenticated</param>
    /// <returns>Json Web Token</returns>
    [AllowAnonymous]
    [OutputCache(PolicyName = nameof(TokenCachePolicy))]
    [HttpGet("getTokenAsync")]
    public async Task<IActionResult> GetTokenAsync(LoginUserQuery loginCommandRequest)
    {
        return Ok(await Mediator.Send(loginCommandRequest));
    }

    [OutputCache(PolicyName = nameof(AuthenticatorBaseCachePolicy))]
    [HttpGet("getAllAsync")]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await Mediator.Send(new GetAllUsersQuery()));
    }

    [OutputCache(PolicyName = nameof(ByIdCachePolicy))]
    [HttpGet("getByIdAsync")]
    public async Task<IActionResult> GetByIdAsync(GetUserByIdQuery getUserByIdRequest)
    {
        return Ok(await Mediator.Send(getUserByIdRequest));
    }
}
