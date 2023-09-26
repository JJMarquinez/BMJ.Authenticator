using BMJ.Authenticator.Api.Exceptions.Strategies.Handlers.Supporters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BMJ.Authenticator.Api.Exceptions.Strategies.Handlers;

public class DefaultExceptionHandlerStrategy : ExceptionHandlerStrategy
{
    private readonly IExceptionSupporter _exceptionSupporter;

    public DefaultExceptionHandlerStrategy(IExceptionSupporter exceptionSupporter)
    {
        _exceptionSupporter = exceptionSupporter;
    }

    public override ObjectResult Handle(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Detail = $"The TraceIdentifier is {context.HttpContext.TraceIdentifier} please contact with IT support team."
        };

        return new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }

    public override bool Support(Exception exception) => _exceptionSupporter.IsSupported<Exception>(exception);
}
