using AutoMapper.Internal;
using BMJ.Authenticator.Application.Common.Models;
using BMJ.Authenticator.Application.Common.Models.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Reflection;

namespace BMJ.Authenticator.Api.Filters;

public class ErrorFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.OK)
        {
            OkObjectResult? result = context.Result as OkObjectResult;
            Type resultValueType = result?.Value?.GetType();
            if (resultValueType is not null 
                && resultValueType.IsGenericType 
                && resultValueType.Name == typeof(ResultDto<>).Name)
            {
                object? success = resultValueType.GetProperty("Success")?.GetValue(result.Value);
                object? errorObject = resultValueType.GetProperty("Error")?.GetValue(result.Value);

                if (success is not null && !(bool)success)
                {
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
            }
        }
    }
}
