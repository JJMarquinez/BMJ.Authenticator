using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace BMJ.Authenticator.Api.Controllers
{
    [ApiController]
    [Route("api/auth/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        private ISender _mediator = null!;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
