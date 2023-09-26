using BMJ.Authenticator.Api.Exceptions.Strategies.Handlers.Supporters;
using BMJ.Authenticator.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BMJ.Authenticator.Api.Exceptions.Strategies.Handlers;

public class ApiValidationExceptionHandlerStrategy : ExceptionHandlerStrategy
{
    private readonly IExceptionSupporter _exceptionSupporter;
        
    public ApiValidationExceptionHandlerStrategy(IExceptionSupporter exceptionSupporter)
    {
        _exceptionSupporter = exceptionSupporter;
    }

    public override ObjectResult Handle(ExceptionContext context)
    {
        ApiValidationException exception = (ApiValidationException)context.Exception;

        var details = new ValidationProblemDetails(exception.Errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Detail = $"If this error should have not happend please contact with IT support team with this TraceIdentifier: {context.HttpContext.TraceIdentifier}."
        };

        return new BadRequestObjectResult(details);
    }

    public override bool Support(Exception exception) => _exceptionSupporter.IsSupported<ApiValidationException>(exception);
}
