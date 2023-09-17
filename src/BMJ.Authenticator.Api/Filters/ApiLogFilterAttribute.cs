using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Instrumentation;
using Microsoft.AspNetCore.Http;
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
            var body = TryGetBody(request);

            _logger.Information("Request before action executes: {TraceIdentifier} {@Request}",
                context.HttpContext.TraceIdentifier,
                new
                {
                    Path = request.Path.Value,
                    request.Method,
                    request.Protocol,
                    Body = body
                });
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            using Activity? loggingActivity = Telemetry.Source.StartActivity("Logging", ActivityKind.Internal);
            loggingActivity!.DisplayName = "Logging request OnActionExecuted";

            var request = context.HttpContext.Request;
            var body = TryGetBody(request);

            _logger.Information("Request and response after action executes: {TraceIdentifier} {@Request} {@Reponse}",
                context.HttpContext.TraceIdentifier,
                new
                {
                    Path = request.Path.Value,
                    request.Method,
                    request.Protocol,
                    Body = body
                },
                new
                {
                    StatusCode = context.Result is not null ? context.HttpContext.Response.StatusCode.ToString() : null,
                    Body = context.Result
                });
            base.OnActionExecuted(context);
        }

        private string TryGetBody(HttpRequest request)
        {
            var body = string.Empty;
            if (request.Body.CanSeek)
            {
                request.Body.Position = 0;
                using (var stream = new StreamReader(request.Body, Encoding.UTF8, true, 1024, leaveOpen: true))
                {
                    body = stream.ReadToEnd();
                }
                request.Body.Position = 0;
            }

            return body;
        }
    }
}
