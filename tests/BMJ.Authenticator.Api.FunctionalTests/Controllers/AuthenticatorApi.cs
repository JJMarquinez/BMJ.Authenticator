namespace BMJ.Authenticator.Api.FunctionalTests.Controllers;

public static class AuthenticatorApi
{
    public static string GetTokenAsync() => "/api/v1/auth/member/getTokenAsync";
    public static string GetAllAsync() => "/api/v1/auth/member/getAllAsync";
}
