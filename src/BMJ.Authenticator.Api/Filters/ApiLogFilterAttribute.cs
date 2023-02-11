using BMJ.Authenticator.Application.Common.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BMJ.Authenticator.Api.Filters
{
    public class ApiLogFilterAttribute : ActionFilterAttribute
    {
        private readonly IAuthLogger _logger;

        public ApiLogFilterAttribute(IAuthLogger logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.Information("Request: {TraceIdentifier} {@Request} {@Reponse}",
                context.HttpContext.TraceIdentifier,
                new
                {
                    Path = context.HttpContext.Request.Path.Value,
                    Method = context.HttpContext.Request.Method,
                    Protocol = context.HttpContext.Request.Protocol
                },
                new
                {
                    StatusCode = context.HttpContext.Response.StatusCode,
                    Body = context.Result
                });
            base.OnActionExecuted(context);
        }
    }
}
