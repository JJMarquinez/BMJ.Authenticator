using BMJ.Authenticator.Api.Exceptions.Strategies.Handlers;

namespace BMJ.Authenticator.Api.Exceptions.Strategies.Factories;

public class ExceptionHandlerStrategyFactory : IExceptionHandlerStrategyFactory
{
    private readonly IReadOnlyList<ExceptionHandlerStrategy> _handlerStrategies;

    public ExceptionHandlerStrategyFactory(IEnumerable<ExceptionHandlerStrategy> handlerStrategies)
    {
        _handlerStrategies = handlerStrategies.ToList().AsReadOnly();
    }

    public ExceptionHandlerStrategy GetStrategy<TException>(TException exception) 
        where TException : Exception
    {
        ExceptionHandlerStrategy strategy = null!;
        var strategies = _handlerStrategies.Where(x => x.Support(exception)).ToList();
        var defacultStrategy = strategies.FirstOrDefault(x => x is DefaultExceptionHandlerStrategy);

        strategies.Remove(defacultStrategy!);

        if (strategies.Count == 0)
            strategy = defacultStrategy!;
        else if (strategies.Count == 1)
            strategy = strategies.First();
        else
            throw new Exception("Ambiguous exception handler strategy to use. There must be just one registered.");

        return strategy;
    }
}
