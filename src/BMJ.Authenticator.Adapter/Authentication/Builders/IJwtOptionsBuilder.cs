namespace BMJ.Authenticator.Adapter.Authentication.Builders;

public interface IJwtOptionsBuilder
{
    IJwtOptionsBuilder WithIssuer(string issuer);
    IJwtOptionsBuilder WithAudience(string audience);
    IJwtOptionsBuilder WithSecretKey(string secretKey);
    JwtOptions Build();
}
