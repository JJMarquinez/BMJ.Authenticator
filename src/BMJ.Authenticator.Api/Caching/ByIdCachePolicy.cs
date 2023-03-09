using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;
using System.Text.Json;

namespace BMJ.Authenticator.Api.Caching;

public class ByIdCachePolicy : AuthenticatorBaseCachePolicy
{
    public static async ValueTask<KeyValuePair<string, string>> VaryByValueAsync(HttpContext context, CancellationToken ct)
    {
        KeyValuePair<string, string> varyBy = new KeyValuePair<string, string>();
        context.Request.EnableBuffering();
        using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
        {
            string? requestId = JsonSerializer.Deserialize<RequestById>(await reader.ReadToEndAsync())?.id?.ToString();

            context.Request.Body.Position = 0;
            varyBy = new KeyValuePair<string, string>(nameof(requestId), requestId);
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

public static partial class OutputCacheOptionsExtensions
{
    public static OutputCacheOptions AddByIdCachePolicy(this OutputCacheOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.AddPolicy(nameof(ByIdCachePolicy), builder =>
        {
            builder.AddPolicy<ByIdCachePolicy>();
            builder.Expire(TimeSpan.FromSeconds(86400));
            builder.VaryByValue(ByIdCachePolicy.VaryByValueAsync);
        });

        return options;
    }
}