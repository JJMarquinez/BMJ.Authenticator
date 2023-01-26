using BMJ.Authenticator.Application.UseCases.Login.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMJ.Authenticator.Api.Controllers
{
    [Authorize]
    public class MemberController : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand loginCommand)
        { 
            return Ok(await Mediator.Send(loginCommand));
        }
    }
}
