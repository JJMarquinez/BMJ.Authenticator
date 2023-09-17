using AutoMapper;
using BMJ.Authenticator.Application.Common.Models;
using BMJ.Authenticator.Domain.Common.Errors;

namespace BMJ.Authenticator.Application.UnitTests.Common.Models.Results;

internal class ResultMappingProfile : Profile
{
    public ResultMappingProfile()
    {
        CreateMap<Error, ErrorDto>();
    }
}
