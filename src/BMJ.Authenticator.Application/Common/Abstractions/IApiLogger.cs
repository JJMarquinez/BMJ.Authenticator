namespace BMJ.Authenticator.Application.Common.Abstractions;

public interface IApiLogger
{
    void Debug(string messageTemplate);
    void Debug<T>(string messageTemplate, T propertyValue);
    void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);
    void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
    void Debug(Exception? exception, string messageTemplate);
    void Debug<T>(Exception? exception, string messageTemplate, T propertyValue);
    void Debug<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);
    void Debug<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
    void Information(string messageTemplate);
    void Information<T>(string messageTemplate, T propertyValue);
    void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);
    void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
    void Information(Exception? exception, string messageTemplate);
    void Information<T>(Exception? exception, string messageTemplate, T propertyValue);
    void Information<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);
    void Information<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
    void Warning(string messageTemplate);
    void Warning<T>(string messageTemplate, T propertyValue);
    void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);
    void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
    void Warning(Exception? exception, string messageTemplate);
    void Warning<T>(Exception? exception, string messageTemplate, T propertyValue);
    void Warning<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);
    void Warning<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
    void Error(string messageTemplate);
    void Error<T>(string messageTemplate, T propertyValue);
    void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);
    void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
    void Error(Exception? exception, string messageTemplate);
    void Error<T>(Exception? exception, string messageTemplate, T propertyValue);
    void Error<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);
    void Error<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
    void Critical(string messageTemplate);
    void Critical<T>(string messageTemplate, T propertyValue);
    void Critical<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);
    void Critical<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
    void Critical(Exception? exception, string messageTemplate);
    void Critical<T>(Exception? exception, string messageTemplate, T propertyValue);
    void Critical<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);
    void Critical<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
}
