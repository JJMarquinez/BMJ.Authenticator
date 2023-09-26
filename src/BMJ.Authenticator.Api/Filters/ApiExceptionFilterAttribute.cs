using Microsoft.AspNetCore.Mvc.Filters;
using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Api.Exceptions.Strategies;

namespace BMJ.Authenticator.Api.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IAuthLogger _logger;
    private readonly IExceptionHandlerStrategyContext _handlerStrategyContext;
    public ApiExceptionFilterAttribute(IAuthLogger logger, IExceptionHandlerStrategyContext handlerStrategyContext)
    {
        _logger = logger;
        _handlerStrategyContext = handlerStrategyContext;
    }

    public override void OnException(ExceptionContext context)
    {
        _logger.Error(context.Exception, "Exception has occurred while executing the request with TraceIdIdentifier: {TraceIdentifier} and exception message: {Message}", context.HttpContext.TraceIdentifier, context.Exception.Message);
        context.Result = _handlerStrategyContext.ExecuteHandling(context);
        context.ExceptionHandled = true;

        base.OnException(context);
    }
}
