using BMJ.Authenticator.Domain;
using System.Collections.Generic;

namespace BMJ.Authenticator.Application.Common.Models;

public class Error
{
    private string code;
    private IEnumerable<string> descriptions;
    private int statusCode;

    private Error(string code, IEnumerable<string> descriptions, int statusCode)
    {
        Ensure.Argument.NotNullOrEmpty(code, nameof(code));
        Ensure.Argument.Is(descriptions != null && descriptions.Count() > 0, nameof(descriptions));
        Ensure.Argument.IsNot(statusCode == 0, nameof(statusCode));
        this.code = code;
        this.descriptions = descriptions;
        this.statusCode = statusCode;
    }

    public static Error Occur(string code, IEnumerable<string> descriptions, int statusCode)
        => new(code, descriptions, statusCode);

    public string GetCode() => code;
    public IEnumerable<string> GetDescriptions() => descriptions;
    public int GetStatusCode() => statusCode;
}
