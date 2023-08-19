using BMJ.Authenticator.Application.Common.Abstractions;
using Microsoft.Extensions.Logging;

namespace BMJ.Authenticator.Infrastructure.Loggers;

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

    public void Debug(string messageTemplate)
    { 
        if(_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug(messageTemplate);
    }

    public void Debug<T>(string messageTemplate, T propertyValue)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug(messageTemplate, propertyValue);
    }

    public void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug(messageTemplate, propertyValue0, propertyValue1);
    }

    public void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    public void Debug(Exception? exception, string messageTemplate)
    {
        if(_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug(exception, messageTemplate);
    }

    public void Debug<T>(Exception? exception, string messageTemplate, T propertyValue)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug(exception, messageTemplate, propertyValue);
    }

    public void Debug<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug(exception, messageTemplate, propertyValue0, propertyValue1);
    }

    public void Debug<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    public void Error(string messageTemplate)
    {
        if (_logger.IsEnabled(LogLevel.Error))
            _logger.LogError(messageTemplate);
    }

    public void Error<T>(string messageTemplate, T propertyValue)
    {
        if (_logger.IsEnabled(LogLevel.Error))
            _logger.LogError(messageTemplate, propertyValue);
    }

    public void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        if (_logger.IsEnabled(LogLevel.Error))
            _logger.LogError(messageTemplate, propertyValue0, propertyValue1);
    }

    public void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        if (_logger.IsEnabled(LogLevel.Error))
            _logger.LogError(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    public void Error(Exception? exception, string messageTemplate)
    {
        if (_logger.IsEnabled(LogLevel.Error))
            _logger.LogError(exception, messageTemplate);
    }

    public void Error<T>(Exception? exception, string messageTemplate, T propertyValue)
    {
        if (_logger.IsEnabled(LogLevel.Error))
            _logger.LogError(exception, messageTemplate, propertyValue);
    }

    public void Error<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        if (_logger.IsEnabled(LogLevel.Error))
            _logger.LogError(exception, messageTemplate, propertyValue0, propertyValue1);
    }

    public void Error<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        if (_logger.IsEnabled(LogLevel.Error))
            _logger.LogError(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    public void Critical(string messageTemplate)
    {
        if (_logger.IsEnabled(LogLevel.Critical))
            _logger.LogCritical(messageTemplate);
    }

    public void Critical<T>(string messageTemplate, T propertyValue)
    {
        if (_logger.IsEnabled(LogLevel.Critical))
            _logger.LogCritical(messageTemplate, propertyValue);
    }

    public void Critical<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        if (_logger.IsEnabled(LogLevel.Critical))
            _logger.LogCritical(messageTemplate, propertyValue0, propertyValue1);
    }

    public void Critical<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        if (_logger.IsEnabled(LogLevel.Critical))
            _logger.LogCritical(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    public void Critical(Exception? exception, string messageTemplate)
    {
        if (_logger.IsEnabled(LogLevel.Critical))
            _logger.LogCritical(exception, messageTemplate);
    }

    public void Critical<T>(Exception? exception, string messageTemplate, T propertyValue)
    {
        if (_logger.IsEnabled(LogLevel.Critical))
            _logger.LogCritical(exception, messageTemplate, propertyValue);
    }

    public void Critical<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        if (_logger.IsEnabled(LogLevel.Critical))
            _logger.LogCritical(exception, messageTemplate, propertyValue0, propertyValue1);
    }

    public void Critical<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        if (_logger.IsEnabled(LogLevel.Critical))
            _logger.LogCritical(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    public void Information(string messageTemplate)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation(messageTemplate);
    }

    public void Information<T>(string messageTemplate, T propertyValue)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation(messageTemplate, propertyValue);
    }

    public void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation(messageTemplate, propertyValue0, propertyValue1);
    }

    public void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    public void Information(Exception? exception, string messageTemplate)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation(exception, messageTemplate);
    }

    public void Information<T>(Exception? exception, string messageTemplate, T propertyValue)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation(exception, messageTemplate, propertyValue);
    }

    public void Information<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation(exception, messageTemplate, propertyValue0, propertyValue1);
    }

    public void Information<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    public void Warning(string messageTemplate)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
            _logger.LogWarning(messageTemplate);
    }

    public void Warning<T>(string messageTemplate, T propertyValue)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
            _logger.LogWarning(messageTemplate, propertyValue);
    }

    public void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
            _logger.LogWarning(messageTemplate, propertyValue0, propertyValue1);
    }

    public void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
            _logger.LogWarning(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    public void Warning(Exception? exception, string messageTemplate)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
            _logger.LogWarning(exception, messageTemplate);
    }

    public void Warning<T>(Exception? exception, string messageTemplate, T propertyValue)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
            _logger.LogWarning(exception, messageTemplate, propertyValue);
    }

    public void Warning<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
            _logger.LogWarning(exception, messageTemplate, propertyValue0, propertyValue1);
    }

    public void Warning<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
            _logger.LogWarning(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }
}
