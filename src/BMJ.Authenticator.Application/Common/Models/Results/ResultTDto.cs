using AutoMapper;
using BMJ.Authenticator.Application.Common.Mappings;
using BMJ.Authenticator.Domain.Common.Results;

namespace BMJ.Authenticator.Application.Common.Models.Results;

public class ResultDto<TValue> : IMapFrom<Result<TValue>>
{
    public bool Success { get; set; }
    public ErrorDto Error { get; set; }
    public TValue Value { get; set; }

    internal static ResultDto<TValue> MakeSuccess<TValue>(TValue value) => new ResultDto<TValue> { Value = value, Success = true, Error = ErrorDto.None };

    internal static ResultDto<TValue> MakeFailure<TValue>(ErrorDto error) => new ResultDto<TValue> { Value = default, Success = false, Error = error };

    public void Mapping(Profile profile)
    {
        profile.CreateMap(typeof(Result<>), typeof(ResultDto<>))
            .ForMember("Success", opt => opt.MapFrom(nameof(Result<object>.IsSuccess)));
    }
}
