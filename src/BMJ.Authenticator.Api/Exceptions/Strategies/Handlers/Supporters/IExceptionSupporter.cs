namespace BMJ.Authenticator.Api.Exceptions.Strategies.Handlers.Supporters;

public interface IExceptionSupporter
{
    bool IsSupported<TException>(Exception exception) where TException : Exception;
}
