using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BMJ.Authenticator.Api.Exceptions.Strategies.Handlers;

public abstract class ExceptionHandlerStrategy
{
    public abstract ObjectResult Handle(ExceptionContext context);
    public abstract bool Support(Exception exception);
}
