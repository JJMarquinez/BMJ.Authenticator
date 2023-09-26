using BMJ.Authenticator.Api.Exceptions.Strategies.Handlers;

namespace BMJ.Authenticator.Api.Exceptions.Strategies.Factories;

public interface IExceptionHandlerStrategyFactory
{
    ExceptionHandlerStrategy GetStrategy<TException>(TException exception) where TException : Exception;
}
