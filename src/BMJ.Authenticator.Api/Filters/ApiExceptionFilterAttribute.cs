using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using BMJ.Authenticator.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using BMJ.Authenticator.Application.Common.Abstractions;

namespace BMJ.Authenticator.Api.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>();
    private readonly IAuthLogger _logger;

    public ApiExceptionFilterAttribute(IAuthLogger logger)
    {
        _logger = logger;
        RegisterExceptionHandler(typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException);
        RegisterExceptionHandler(typeof(AuthException), HandleAuthException);
    }

    private void RegisterExceptionHandler(Type type, Action<ExceptionContext> handler)
    { 
        _exceptionHandlers.Add(type, handler);
    }

    public override void OnException(ExceptionContext context)
    {
        _logger.Error(context.Exception, context.Exception.Message);
        HandleException(context);

        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }
    }

    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        var details = new ValidationProblemDetails(context.ModelState)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status401Unauthorized
        };

        context.ExceptionHandled = true;
    }

    private void HandleAuthException(ExceptionContext context)
    {
        AuthException exception = (AuthException)context.Exception;

        var details = new ProblemDetails
        {
            Status = exception.GetError().GetStatusCodeAsInt(),
            Title = exception.Message,
            
        };
        details.Extensions.Add(
            "errors", 
            new Dictionary<string, object>() {
                { 
                    exception.GetError().GetCode(),
                    new {
                        descriptions = exception.GetError().GetDescriptions().ToArray(), 
                        errorType = exception.GetError().GetErrorType().ToString()
                    }
                }
            });

        context.Result = new ObjectResult(details)
        {
            StatusCode = exception.GetError().GetStatusCodeAsInt()
        };

        context.ExceptionHandled = true;
    }
}
