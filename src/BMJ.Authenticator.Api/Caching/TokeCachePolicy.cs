using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;
using System.Text.Json;

namespace BMJ.Authenticator.Api.Caching;

public class TokenCachePolicy : AuthenticatorBaseCachePolicy
{
    public static async ValueTask<KeyValuePair<string, string>> VaryByValueAsync(HttpContext context, CancellationToken ct)
    {
        KeyValuePair<string, string> varyBy = new KeyValuePair<string, string>();
        context.Request.EnableBuffering();
        using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
        {
            var tokeRequest = JsonSerializer.Deserialize<TokenRequest>(await reader.ReadToEndAsync());
            string? userName = tokeRequest!.userName;
            string? password = tokeRequest!.password;
            context.Request.Body.Position = 0;
            varyBy = new KeyValuePair<string, string>(nameof(TokenRequest), string.Concat(userName, password));
        }
        return varyBy;
    }
}

internal class TokenRequest
{ 
    public string? userName { get; set; }
    public string? password { get; set; }
}

public static partial class OutputCacheOptionsExtensions
{
    public static OutputCacheOptions AddTokenCachePolicy(this OutputCacheOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.AddPolicy(nameof(TokenCachePolicy), builder =>
        {
            builder.AddPolicy<TokenCachePolicy>();
            builder.Expire(TimeSpan.FromSeconds(900));
            builder.VaryByValue(TokenCachePolicy.VaryByValueAsync);
        });

        return options;
    }
}
