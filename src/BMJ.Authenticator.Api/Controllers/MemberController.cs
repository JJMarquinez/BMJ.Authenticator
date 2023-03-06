using BMJ.Authenticator.Api.Caching;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser;
using BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser;
using BMJ.Authenticator.Application.UseCases.Users.Commands.LoginUser;
using BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace BMJ.Authenticator.Api.Controllers;

[Authorize]
public class MemberController : ApiControllerBase
{
    [AllowAnonymous]
    [OutputCache(PolicyName = nameof(AuthorizationCachePolicy), Duration = 900)]
    [HttpPost("loginAsync")]
    public async Task<IActionResult> LoginAsync(LoginUserCommandRequest loginCommandRequest)
    { 
        return Ok(await Mediator.Send(loginCommandRequest));
    }

    [OutputCache(PolicyName = nameof(AuthorizationCachePolicy))]
    [HttpGet("getAllAsync")]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await Mediator.Send(new GetAllUsersQueryRequest()));
    }

    [OutputCache(PolicyName = nameof(ByIdCachePolicy))]
    [HttpGet("getByIdAsync")]
    public async Task<IActionResult> GetByIdAsync(GetUserByIdQueryRequest getUserByIdRequest)
    {
        return Ok(await Mediator.Send(getUserByIdRequest));
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost("createAsync")]
    public async Task<IActionResult> CreateAsync(CreateUserCommandRequest createUserCommandRequest)
    {
        return Ok(await Mediator.Send(createUserCommandRequest));
    }

    [Authorize(Roles = "Administrator")]
    [HttpPut("updateAsync")]
    public async Task<IActionResult> UpdateAsync(UpdateUserCommandRequest updateUserCommandRequest, CancellationToken ct)
    {
        ResultDto result = await Mediator.Send(updateUserCommandRequest);
        if(result.Success)
            await Cache.EvictByTagAsync(updateUserCommandRequest.Id, ct);
        return Ok(result);
    }

    [Authorize(Roles = "Administrator")]
    [HttpDelete("deleteAsync")]
    public async Task<IActionResult> DeleteAsync(DeleteUserCommandRequest deleteUserCommandRequest)
    {
        return Ok(await Mediator.Send(deleteUserCommandRequest));
    }
}
