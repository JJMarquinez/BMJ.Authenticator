using AutoMapper;
using BMJ.Authenticator.Application.Common.Mappings;
using BMJ.Authenticator.Domain.Common;

namespace BMJ.Authenticator.Application.Common.Models;

public class ErrorDto : IMapFrom<Error>
{
    public string Code { get; set; }

    public string Title { get; set; }

    public string Detail { get; set; }

    public int HttpStatusCode { get; set; }
}
