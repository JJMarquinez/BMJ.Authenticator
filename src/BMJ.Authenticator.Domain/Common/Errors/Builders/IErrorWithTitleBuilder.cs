namespace BMJ.Authenticator.Domain.Common.Errors.Builders;

public interface IErrorWithTitleBuilder
{
    IErrorWithDetailBuilder WithTitle(string title);
}
