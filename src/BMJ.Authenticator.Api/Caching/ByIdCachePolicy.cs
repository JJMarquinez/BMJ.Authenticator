using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;
using System.Text.Json;

namespace BMJ.Authenticator.Api.Caching;

public class ByIdCachePolicy : AuthenticatorBaseCachePolicy
{
    public static KeyValuePair<string, string> VaryByValue(HttpContext context)
    {
        KeyValuePair<string, string> varyBy = new KeyValuePair<string, string>();
        IHttpBodyControlFeature syncIOFeature = context.Features.Get<IHttpBodyControlFeature>();
        if (syncIOFeature is not null)
        {
            context.Request.EnableBuffering();
            syncIOFeature.AllowSynchronousIO = true;
            using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
            {
                string? requestId = JsonSerializer.Deserialize<RequestById>(reader.ReadToEnd())?.id?.ToString();

                syncIOFeature.AllowSynchronousIO = false;
                context.Request.Body.Position = 0;
                varyBy = new KeyValuePair<string, string>(nameof(requestId), requestId);
            }
        }
        return varyBy;
    }

    protected override async ValueTask AddTagsAsync(OutputCacheContext context)
    {
        context.HttpContext.Request.EnableBuffering();
        using (var reader = new StreamReader(context.HttpContext.Request.Body, leaveOpen: true))
        {
            string? requestId = JsonSerializer.Deserialize<RequestById>(await reader.ReadToEndAsync())?.id?.ToString();

            if (requestId is not null)
                context.Tags.Add(requestId);

            context.HttpContext.Request.Body.Position = 0;
        }
    }
}

internal class RequestById
{ 
    public string? id { get; set; }
}
