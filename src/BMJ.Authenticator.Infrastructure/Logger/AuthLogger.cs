using BMJ.Authenticator.Application.Common.Abstractions;
using Microsoft.Extensions.Logging;

namespace BMJ.Authenticator.Infrastructure.Logger;

public sealed class BMJAuthenticator
{
}

public class AuthLogger : IAuthLogger
{
    private readonly ILogger _logger;

    public AuthLogger(ILogger<BMJAuthenticator> logger)
    {
        _logger = logger;
    }

    public void LogError(string message, params object[] args)
    {
        _logger.LogError(message, args);
    }

    public void LogError(Exception exception, string message, params object[] args)
    {
        _logger.LogError(exception, message, args);
    }

    public void LogError(Exception exception)
    {
        _logger.LogError($"Error: {exception}");    
    }

    public void LogInformation(string message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }
}
