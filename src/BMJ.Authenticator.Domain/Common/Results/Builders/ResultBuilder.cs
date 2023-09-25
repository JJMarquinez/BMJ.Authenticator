using BMJ.Authenticator.Domain.Common.Errors;

namespace BMJ.Authenticator.Domain.Common.Results.Builders;

public class ResultBuilder : IResultBuilder, IResultErrorBuilder
{
    private Error _error = null!;
    public Result Build() => Result.MakeFailure(_error);

    public Result BuildSuccess() => Result.MakeSuccess();

    public IResultErrorBuilder WithError(Error error)
    {
        _error = error;
        return this;
    }
}
