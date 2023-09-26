using FluentValidation.Results;

namespace BMJ.Authenticator.Application.Common.Exceptions;

public class ApiValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ApiValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ApiValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }
}
