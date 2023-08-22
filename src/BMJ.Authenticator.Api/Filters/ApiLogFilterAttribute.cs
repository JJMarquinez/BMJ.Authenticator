using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Instrumentation;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Text;

namespace BMJ.Authenticator.Api.Filters
{
    public class ApiLogFilterAttribute : ActionFilterAttribute
    {
        private readonly IAuthLogger _logger;

        public ApiLogFilterAttribute(IAuthLogger logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            using Activity? loggingActivity = Telemetry.Source.StartActivity("Logging", ActivityKind.Internal);
            loggingActivity!.DisplayName = "Logging request OnActionExecuting";

            var request = context.HttpContext.Request;
            var body = string.Empty;
            request.Body.Position = 0;
            using (var stream = new StreamReader(request.Body, Encoding.UTF8, true, 1024, leaveOpen: true))
            {
                body = stream.ReadToEnd();
            }
            request.Body.Position = 0;

            _logger.Information("Request before action executes: {TraceIdentifier} {@Request}",
                context.HttpContext.TraceIdentifier,
                new
                {
                    Path = context.HttpContext.Request.Path.Value,
                    context.HttpContext.Request.Method,
                    context.HttpContext.Request.Protocol,
                    Body = body
                });
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            using Activity? loggingActivity = Telemetry.Source.StartActivity("Logging", ActivityKind.Internal);
            loggingActivity.DisplayName = "Logging request OnActionExecuted";

            _logger.Information("Request and response after action executes: {TraceIdentifier} {@Request} {@Reponse}",
                context.HttpContext.TraceIdentifier,
                new
                {
                    Path = context.HttpContext.Request.Path.Value,
                    context.HttpContext.Request.Method,
                    context.HttpContext.Request.Protocol
                },
                new
                {
                    StatusCode = context.Result is not null ? context.HttpContext.Response.StatusCode.ToString() : null,
                    Body = context.Result
                });
            base.OnActionExecuted(context);
        }
    }
}
