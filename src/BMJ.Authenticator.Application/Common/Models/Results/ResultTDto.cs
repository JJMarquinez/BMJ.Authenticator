using AutoMapper;
using BMJ.Authenticator.Application.Common.Mappings;
using BMJ.Authenticator.Domain.Common.Results;

namespace BMJ.Authenticator.Application.Common.Models.Results;

public class ResultDto<TValue> : IMapFrom<Result<TValue>>
{
    public bool Success { get; set; }
    public ErrorDto Error { get; set; }
    public TValue Value { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap(typeof(Result<>), typeof(ResultDto<>))
            .ForMember("Success", opt => opt.MapFrom(nameof(Result<object>.IsSuccess)));
    }
}
