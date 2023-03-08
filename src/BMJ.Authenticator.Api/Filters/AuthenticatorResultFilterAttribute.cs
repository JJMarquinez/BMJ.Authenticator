using BMJ.Authenticator.Application.Common.Models;
using BMJ.Authenticator.Application.Common.Models.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace BMJ.Authenticator.Api.Filters;

public class AuthenticatorResultFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.OK)
        {
            OkObjectResult? result = context.Result as OkObjectResult;
            Type resultValueType = result?.Value?.GetType();
            IEnumerable<string> resultDtoTypes = new List<string> { typeof(ResultDto<>).Name, typeof(ResultDto).Name };
            if (resultValueType is not null
                && resultDtoTypes.Contains(resultValueType.Name, StringComparer.Ordinal))
            {
                object? success = resultValueType.GetProperty("Success")?.GetValue(result.Value);

                if (success is not null)
                {
                    if ((bool)success)
                        SuccessResultHandler(context, result, resultValueType);
                    else
                        ErrorResultHandler(context, result, resultValueType);
                }
            }
        }
    }

    private void ErrorResultHandler(ActionExecutedContext context, OkObjectResult result, Type resultValueType)
    {
        object? errorObject = resultValueType.GetProperty("Error")?.GetValue(result.Value);
        if (errorObject is not null)
        {
            ErrorDto error = (ErrorDto)errorObject;
            ProblemDetails detail = new ProblemDetails()
            {
                Instance = context.HttpContext.Request.Path.Value,
                Title = error.Title,
                Detail = error.Detail,
                Status = error.HttpStatusCode
            };

            detail.Extensions.Add("errorCode", error.Code);

            context.Result = new ObjectResult(detail)
            {
                StatusCode = error.HttpStatusCode
            };
        }
    }

    private void SuccessResultHandler(ActionExecutedContext context, OkObjectResult result, Type resultValueType)
    {
        if (string.Equals(resultValueType.Name, typeof(ResultDto<>).Name, StringComparison.Ordinal))
        {
            object? value = resultValueType.GetProperty("Value")?.GetValue(result.Value);
            context.Result = new OkObjectResult(value);
        }
        else if (string.Equals(resultValueType.Name, typeof(ResultDto).Name, StringComparison.Ordinal))
        {
            context.Result = new NoContentResult();
        }
    }
}
