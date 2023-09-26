namespace BMJ.Authenticator.Api.Exceptions.Strategies.Handlers.Supporters;

public class ExceptionSupporter : IExceptionSupporter
{
    public bool IsSupported<TException>(Exception exception) where TException : Exception
        => typeof(TException).IsAssignableFrom(exception.GetType());
}
