namespace BMJ.Authenticator.Application.Common.Abstractions;

public interface IAuthLogger
{
    void LogInformation(string message, params object[] args);
    void LogError(string message, params object[] args);
    void LogError(Exception exception, string message, params object[] args);
    void LogError(Exception exception);
}