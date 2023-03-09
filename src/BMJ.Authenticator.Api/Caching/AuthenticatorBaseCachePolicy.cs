using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace BMJ.Authenticator.Api.Caching;

public class AuthenticatorBaseCachePolicy : IOutputCachePolicy
{
    public virtual async ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        await AddTagsAsync(context);
        var attemptOutputCaching = AttemptOutputCaching(context);
        context.EnableOutputCaching = true;
        context.AllowCacheLookup = attemptOutputCaching;
        context.AllowCacheStorage = attemptOutputCaching;
        context.AllowLocking = true;

        // Vary by any query by default
        context.CacheVaryByRules.QueryKeys = "*";
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

    protected virtual async ValueTask AddTagsAsync(OutputCacheContext context)
    {
        string? endpointName = context.HttpContext.Request.Path.Value?.Substring(context.HttpContext.Request.Path.Value.LastIndexOf("/", StringComparison.Ordinal) + 1);
        if (!string.IsNullOrEmpty(endpointName))
            context.Tags.Add(endpointName);
    }
}

public static partial class OutputCacheOptionsExtensions
{
    public static OutputCacheOptions AddAuthenticatorBaseCachePolicy(this OutputCacheOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.AddPolicy(nameof(AuthenticatorBaseCachePolicy), builder =>
        {
            builder.AddPolicy<AuthenticatorBaseCachePolicy>();
            builder.Expire(TimeSpan.FromSeconds(86400));
        });

        return options;
    }
}
