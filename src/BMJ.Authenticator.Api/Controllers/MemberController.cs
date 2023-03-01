using BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser;
using BMJ.Authenticator.Application.UseCases.Users.Commands.LoginUser;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserByName;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMJ.Authenticator.Api.Controllers;

[Authorize]
public class MemberController : ApiControllerBase
{
    [AllowAnonymous]
    [HttpPost("loginAsync")]
    public async Task<IActionResult> LoginAsync(LoginUserCommandRequest loginCommandRequest)
    { 
        return Ok(await Mediator.Send(loginCommandRequest));
    }

    [HttpGet("getAllAsync")]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await Mediator.Send(new GetAllUsersQueryRequest()));
    }

    [HttpGet("getByNameAsync")]
    public async Task<IActionResult> GetByNameAsync(GetUserByNameQueryRequest getUserByNameRequest)
    {
        return Ok(await Mediator.Send(getUserByNameRequest));
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost("createAsync")]
    public async Task<IActionResult> CreateAsync(CreateUserCommandRequest createUserCommandRequest)
    {
        return Ok(await Mediator.Send(createUserCommandRequest));
    }
}
