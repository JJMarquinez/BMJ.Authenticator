using AutoMapper;
using BMJ.Authenticator.Application.Common.Mappings;
using BMJ.Authenticator.Domain.Common;

namespace BMJ.Authenticator.Application.Common.Models;

public class ErrorDto : IMapFrom<Error>
{
    public string Code { get; set; }

    public string Message { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Error, ErrorDto>();
    }
}
