﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BMJ.Authenticator.Adapter.Common.Abstractions;

namespace BMJ.Authenticator.Api.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>();
    private readonly IAuthLogger _logger;

    public ApiExceptionFilterAttribute(IAuthLogger logger)
    {
        _logger = logger;
        RegisterExceptionHandler(typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException);
    }

    private void RegisterExceptionHandler(Type type, Action<ExceptionContext> handler)
    { 
        _exceptionHandlers.Add(type, handler);
    }

    public override void OnException(ExceptionContext context)
    {
        _logger.Error(context.Exception, "Exception has occurred while executing the request with TraceIdIdentifier: {TraceIdentifier} and exception message: {Message}", context.HttpContext.TraceIdentifier, context.Exception.Message);
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
        else
        {
            HandleUnHandledException(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }
    }

    private static void HandleUnHandledException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Detail = $"The TraceIdentifier is {context.HttpContext.TraceIdentifier} please contact with IT support team."
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

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
}
