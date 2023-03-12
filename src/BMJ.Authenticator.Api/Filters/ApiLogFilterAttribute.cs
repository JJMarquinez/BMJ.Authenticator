using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Instrumentation;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

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
            using Activity? loggingActivity = Telemetry.Source.StartActivity("Logging", System.Diagnostics.ActivityKind.Internal);
            loggingActivity.DisplayName = "Logging request";

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
