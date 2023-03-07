using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace BMJ.Authenticator.Api.Caching;

public class TokenCachePolicy : AuthenticatorBaseCachePolicy
{
    public static async ValueTask<KeyValuePair<string, string>> VaryByValue(HttpContext context, CancellationToken ct)
    {
        KeyValuePair<string, string> varyBy = new KeyValuePair<string, string>();
        context.Request.EnableBuffering();
        using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
        {
            string? userName = JsonSerializer.Deserialize<RequestByName>(await reader.ReadToEndAsync())?.userName?.ToString();
            context.Request.Body.Position = 0;
            varyBy = new KeyValuePair<string, string>(nameof(userName), userName);
        }
        return varyBy;
    }
}

internal class RequestByName
{ 
    public string? userName { get; set; }
}
