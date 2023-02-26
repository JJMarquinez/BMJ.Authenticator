using BMJ.Authenticator.Application.UseCases.Users.Commands.LoginUser;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMJ.Authenticator.Api.Controllers;

[Authorize]
public class MemberController : ApiControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommandRequest loginCommand)
    { 
        return Ok(await Mediator.Send(loginCommand));
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await Mediator.Send(new GetAllUsersQueryRequest()));
    }
}
