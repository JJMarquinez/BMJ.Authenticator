using AutoMapper;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Users;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.LoginUser;

public class LoginUserCommandRequestHandler
    : IRequestHandler<LoginUserCommandRequest, ResultDto<string?>>
{
    private readonly IIdentityAdapter _identityAdapter;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;
    public LoginUserCommandRequestHandler(IIdentityAdapter identityService, IJwtProvider jwtProvider, IMapper mapper)
    {
        _identityAdapter = identityService;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
    }

    public async Task<ResultDto<string?>> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
    {
        ResultDto<UserDto?> userResult = await _identityAdapter.AuthenticateMemberAsync(request.UserName, request.Password);

        return userResult.Success
            ? ResultDto<string?>.NewSuccess<string?>(_jwtProvider.Generate(userResult.Value!))
            : ResultDto<string?>.NewFailure<string?>(userResult.Error);
    }
}
