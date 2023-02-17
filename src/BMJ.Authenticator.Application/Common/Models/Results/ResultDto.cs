using AutoMapper;
using BMJ.Authenticator.Application.Common.Mappings;
using BMJ.Authenticator.Domain.Common.Results;

namespace BMJ.Authenticator.Application.Common.Models.Results;

public class ResultDto : IMapFrom<Result>
{
    public bool Success { get; set; }
    public ErrorDto Error { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Result, ResultDto>()
            .ForMember(d => d.Success, opt => opt.MapFrom(s => s.IsSuccess()));
    }
}
