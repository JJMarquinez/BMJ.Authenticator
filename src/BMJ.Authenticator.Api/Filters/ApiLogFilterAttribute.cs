using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace BMJ.Authenticator.Api.Filters
{
    public class ApiLogFilterAttribute : ActionFilterAttribute
    {
        private readonly ILogger _logger;

        public ApiLogFilterAttribute(ILogger<ActionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("BMJ.Authentidator Request: {Name} {@Request}",
                nameof(context.HttpContext.Request), context.HttpContext.Request.RouteValues);
            base.OnActionExecuting(context);
        }
    }
}
