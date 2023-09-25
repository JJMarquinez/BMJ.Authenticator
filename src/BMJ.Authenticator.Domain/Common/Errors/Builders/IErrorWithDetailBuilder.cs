namespace BMJ.Authenticator.Domain.Common.Errors.Builders;

public interface IErrorWithDetailBuilder
{
    IErrorWithHttpStatusCodeBuilder WithDetail(string detail);
}
