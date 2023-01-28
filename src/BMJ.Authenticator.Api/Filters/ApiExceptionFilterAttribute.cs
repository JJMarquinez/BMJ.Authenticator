using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using BMJ.Authenticator.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BMJ.Authenticator.Api.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>();

    public ApiExceptionFilterAttribute()
    {
        RegisterExceptionHandler(typeof(ValidationException), HandleValidationException);
        RegisterExceptionHandler(typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException);
        RegisterExceptionHandler(typeof(AuthException), HandleAuthException);
    }

    private void RegisterExceptionHandler(Type type, Action<ExceptionContext> handler)
    { 
        _exceptionHandlers.Add(type, handler);
    }

    public override void OnException(ExceptionContext context)
    {
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

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;

        var details = new ValidationProblemDetails(exception.Errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
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
        var exception = (AuthException)context.Exception;

        var details = new ProblemDetails
        {
            Status = exception.GetError().GetStatusCode(),
            Title = exception.Message,
        };
        details.Extensions.Add("InternalErrorCode", exception.GetError().GetCode());
        details.Extensions.Add("Details", exception.GetError().GetDescriptions());

        context.Result = new ObjectResult(details)
        {
            StatusCode = exception.GetError().GetStatusCode()
        };

        context.ExceptionHandled = true;
    }
}
