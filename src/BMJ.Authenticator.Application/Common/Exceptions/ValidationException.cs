using FluentValidation.Results;

namespace BMJ.Authenticator.Application.Common.Exceptions;

public class ValidationException : Exception
{
    private IDictionary<string, string[]> errors;
    private ValidationException()
        : base("One or more validation failures have occurred.")
    {
        errors = new Dictionary<string, string[]>();
    }

    private ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public static ValidationException New(IEnumerable<ValidationFailure> failures)
        => new (failures);

    public IDictionary<string, string[]> GetErrors() => errors;
}
