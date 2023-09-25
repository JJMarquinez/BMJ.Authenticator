using AutoMapper;
using BMJ.Authenticator.Application.Common.Mappings;
using BMJ.Authenticator.Application.Common.Models.Errors;
using BMJ.Authenticator.Domain.Common;
using BMJ.Authenticator.Domain.Common.Results;

namespace BMJ.Authenticator.Application.Common.Models.Results;

public class ResultDto : IMapFrom<Result>
{

    public bool Success { get; set; }
    public ErrorDto Error { get; set; }

    internal static ResultDto MakeSuccess() => new ResultDto { Success = true, Error = ErrorDto.None };

    internal static ResultDto MakeFailure(ErrorDto error)
    {
        Ensure.Argument.IsNot(error is null || error == ErrorDto.None, "The failure result cannot be implemented with no error");
        return new ResultDto { Success = false, Error = error };
    }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Result, ResultDto>()
            .ForMember(d => d.Success, opt => opt.MapFrom(s => s.IsSuccess()));
    }
}
