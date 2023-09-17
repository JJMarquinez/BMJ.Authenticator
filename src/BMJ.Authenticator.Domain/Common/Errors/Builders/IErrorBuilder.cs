namespace BMJ.Authenticator.Domain.Common.Errors.Builders;

public interface IErrorBuilder
{
    IErrorWithTitleBuilder WithCode(string code);
}