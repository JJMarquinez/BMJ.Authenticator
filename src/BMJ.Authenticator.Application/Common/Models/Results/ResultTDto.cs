using AutoMapper;
using BMJ.Authenticator.Application.Common.Mappings;
using BMJ.Authenticator.Application.Common.Models.Errors;
using BMJ.Authenticator.Domain.Common;
using BMJ.Authenticator.Domain.Common.Results;

namespace BMJ.Authenticator.Application.Common.Models.Results;

public class ResultDto<TValue> : IMapFrom<Result<TValue>>
{
    public bool Success { get; set; }
    public ErrorDto Error { get; set; }
    public TValue Value { get; set; }

    internal static ResultDto<TValue> MakeSuccess(TValue value)
    {
        Ensure.Argument.NotNull(value, string.Format("{0} cannot be null.", nameof(value)));
        return new ResultDto<TValue> { Value = value, Success = true, Error = ErrorDto.None };
    }

    internal static ResultDto<TValue> MakeFailure(ErrorDto error)
    {
        Ensure.Argument.IsNot(error is null || error == ErrorDto.None, "The failure result cannot be implemented with no error");
        return new ResultDto<TValue> { Value = default!, Success = false, Error = error };
    }

    public void Mapping(Profile profile)
    {
        profile.CreateMap(typeof(Result<>), typeof(ResultDto<>))
            .ForMember("Success", opt => opt.MapFrom(nameof(Result<object>.IsSuccess)));
    }
}
