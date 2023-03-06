using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;
using System.Text.Json;

namespace BMJ.Authenticator.Api.Caching;

public class ByIdCachePolicy : AuthorizationCachePolicy
{
    public static readonly ByIdCachePolicy Instance = new();

    private ByIdCachePolicy()
    {
    }
    public override ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        /*
         * Each server has a AllowSynchronousIO option that controls this behavior and the default for all of them is now false
         * because it's source of thread starvation and application hangs.
         * 
         * Let's override that behavior per request instead:
        */
        IHttpBodyControlFeature syncIOFeature = context.HttpContext.Features.Get<IHttpBodyControlFeature>();
        if (syncIOFeature is null)
            return ValueTask.CompletedTask;
        else
        {
            context.HttpContext.Request.EnableBuffering();
            syncIOFeature.AllowSynchronousIO = true;
            using (var reader = new StreamReader(context.HttpContext.Request.Body, leaveOpen: true))
            {
                string? requestId = JsonSerializer.Deserialize<RequestById>(reader.ReadToEnd())?.id?.ToString();

                if (requestId is null)
                    return ValueTask.CompletedTask;
                else context.Tags.Add(requestId);

                syncIOFeature.AllowSynchronousIO = false;
                context.HttpContext.Request.Body.Position = 0;
            }
        }

        return base.CacheRequestAsync(context, cancellation);
    }
}

internal class RequestById
{ 
    public string? id { get; set; }
}
