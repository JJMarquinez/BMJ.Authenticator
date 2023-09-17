namespace BMJ.Authenticator.Domain.Common.Errors.Builders;

public interface IErrorWithHttpStatusCodeBuilder
{
    IErrorBuildBuilder WithHttpStatusCode(int httpStatusCode);
}
