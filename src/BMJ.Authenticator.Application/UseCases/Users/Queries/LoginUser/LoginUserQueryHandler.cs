using AutoMapper;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Users;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.LoginUser;

public class LoginUserQueryHandler
    : IRequestHandler<LoginUserQuery, ResultDto<string?>>
{
    private readonly IIdentityAdapter _identityAdapter;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;
    public LoginUserQueryHandler(IIdentityAdapter identityAdapter, IJwtProvider jwtProvider, IMapper mapper)
    {
        _identityAdapter = identityAdapter;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
    }

    public async Task<ResultDto<string?>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var userResultDto = await _identityAdapter.AuthenticateMemberAsync(request.UserName!, request.Password!);
        ResultDto<string?> response;

        if (userResultDto.Success)
        {
            userResultDto.Value = _mapper.Map<UserDto>(userResultDto.Value!.ToUser());
            response = ResultDto<string?>.NewSuccess<string?>(_jwtProvider.Generate(userResultDto.Value!));
        }
        else
            response = ResultDto<string?>.NewFailure<string?>(userResultDto.Error);

        return response;
    }
}
