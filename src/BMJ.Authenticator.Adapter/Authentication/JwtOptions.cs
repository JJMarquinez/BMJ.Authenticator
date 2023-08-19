using BMJ.Authenticator.Adapter.Authentication.Builders;

namespace BMJ.Authenticator.Adapter.Authentication
{
    public class JwtOptions
    {
        public string Issuer { get; init; }
        public string Audience { get; init; }
        public string SecretKey { get; init; }

        public static IJwtOptionsBuilder Builder() => JwtOptionsBuilder.New();
    }
}
