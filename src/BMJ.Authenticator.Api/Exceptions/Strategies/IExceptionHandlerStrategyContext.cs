using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BMJ.Authenticator.Api.Exceptions.Strategies;

public interface IExceptionHandlerStrategyContext
{
    ObjectResult ExecuteHandling(ExceptionContext context);
}
