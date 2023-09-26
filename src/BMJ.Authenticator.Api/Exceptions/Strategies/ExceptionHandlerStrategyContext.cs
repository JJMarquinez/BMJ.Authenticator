using BMJ.Authenticator.Api.Exceptions.Strategies.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BMJ.Authenticator.Api.Exceptions.Strategies;

public class ExceptionHandlerStrategyContext : IExceptionHandlerStrategyContext
{
    private readonly IExceptionHandlerStrategyFactory _handlerStrategyFactory;

    public ExceptionHandlerStrategyContext(IExceptionHandlerStrategyFactory handlerStrategyFactory)
    {
        _handlerStrategyFactory = handlerStrategyFactory;
    }

    public ObjectResult ExecuteHandling(ExceptionContext context)
    {
        var exceptionHandler = _handlerStrategyFactory.GetStrategy(context.Exception);
        return exceptionHandler.Handle(context);
    }
}
