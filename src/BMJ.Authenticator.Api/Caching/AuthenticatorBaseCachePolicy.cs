﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;

namespace BMJ.Authenticator.Api.Caching;

public class AuthenticatorBaseCachePolicy : IOutputCachePolicy
{
    public virtual ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        AddTags(context);
        var attemptOutputCaching = AttemptOutputCaching(context);
        context.EnableOutputCaching = true;
        context.AllowCacheLookup = attemptOutputCaching;
        context.AllowCacheStorage = attemptOutputCaching;
        context.AllowLocking = true;

        // Vary by any query by default
        context.CacheVaryByRules.QueryKeys = "*";

        return ValueTask.CompletedTask;
    }

    public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        return ValueTask.CompletedTask;
    }

    public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        var response = context.HttpContext.Response;

        // Verify existence of cookie headers
        if (!StringValues.IsNullOrEmpty(response.Headers.SetCookie))
        {
            context.AllowCacheStorage = false;
            return ValueTask.CompletedTask;
        }

        // Check response code
        if (response.StatusCode != StatusCodes.Status200OK)
        {
            context.AllowCacheStorage = false;
            return ValueTask.CompletedTask;
        }

        return ValueTask.CompletedTask;
    }

    protected virtual bool AttemptOutputCaching(OutputCacheContext context)
    {
        // Check if the current request fulfills the requirements
        // to be cached
        var request = context.HttpContext.Request;

        // Verify the method
        if (!HttpMethods.IsGet(request.Method) &&
            !HttpMethods.IsPost(request.Method))
        {
            return false;
        }

        return true;
    }

    protected virtual void AddTags(OutputCacheContext context)
    {
        string? endpointName = context.HttpContext.Request.Path.Value?.Substring(context.HttpContext.Request.Path.Value.LastIndexOf("/", StringComparison.Ordinal) + 1);
        if (!string.IsNullOrEmpty(endpointName))
            context.Tags.Add(endpointName);
    }
}