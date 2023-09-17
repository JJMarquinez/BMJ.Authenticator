namespace BMJ.Authenticator.Domain.Common.Errors;

public interface IErrorBuilder
{
    IErrorWithTitleBuilder WithCode(string code);
}

public interface IErrorWithTitleBuilder
{
    IErrorWithDetailBuilder WithTitle(string title);
}

public interface IErrorWithDetailBuilder
{
    IErrorWithHttpStatusCodeBuilder WithDetail(string detail);
}

public interface IErrorWithHttpStatusCodeBuilder
{
    IErrorBuildBuilder WithHttpStatusCode(int httpStatusCode);
}

public interface IErrorBuildBuilder
{
    Error Build();
}