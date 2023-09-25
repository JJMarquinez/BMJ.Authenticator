using BMJ.Authenticator.Domain.Common.Errors;

namespace BMJ.Authenticator.Domain.Common.Results.Builders;

public interface IResultBuilder
{
    Result BuildSuccess();

    IResultErrorBuilder WithError(Error error);
}

public interface IResultErrorBuilder
{
    Result Build();
}
