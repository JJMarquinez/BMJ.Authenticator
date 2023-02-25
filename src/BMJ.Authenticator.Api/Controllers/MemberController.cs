using BMJ.Authenticator.Application.UseCases.Users.Commands.LoginUser;
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

    //[HttpPost("getAll")]
    //public async Task<IActionResult> GetAll()
    //{
    //    return Ok(await Mediator.Send(new GetAllUserQueryRequest()));
    //}
}
