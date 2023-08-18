namespace BMJ.Authenticator.Infrastructure.Authentication
{
    public class JwtOptions
    {
        public string Issuer { get; init; }
        public string Audience { get; init; }
        public string SecretKey { get; init; }

        public static IJwtOptionsBuilder Builder() => JwtOptionsBuilder.New();
    }

    public interface IJwtOptionsBuilder
    {
        IJwtOptionsBuilder WithIssuer(string issuer);
        IJwtOptionsBuilder WithAudience(string audience);
        IJwtOptionsBuilder WithSecretKey(string secretKey);
        JwtOptions Build();
    }

    public class JwtOptionsBuilder : IJwtOptionsBuilder
    {
        private string _issuer = null!;
        private string _audience = null!;
        private string _secretKey = null!;

        private JwtOptionsBuilder() { }

        public static JwtOptionsBuilder New() => new();

        public JwtOptions Build() 
            => new JwtOptions()
            {  
                Issuer = _issuer,
                Audience = _audience,
                SecretKey = _secretKey
            };

        public IJwtOptionsBuilder WithAudience(string audience)
        {
            _audience = audience;
            return this;
        }

        public IJwtOptionsBuilder WithIssuer(string issuer)
        {
           _issuer = issuer;
            return this;
        }

        public IJwtOptionsBuilder WithSecretKey(string secretKey)
        {
            _secretKey = secretKey;
            return this;
        }
    }
}
