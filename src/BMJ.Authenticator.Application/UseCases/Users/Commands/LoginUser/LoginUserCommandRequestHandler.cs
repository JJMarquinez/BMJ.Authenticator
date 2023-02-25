using AutoMapper;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Interfaces;
using BMJ.Authenticator.Application.Common.Models;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Domain.Common.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.LoginUser;

public class LoginUserCommandRequestHandler
    : IRequestHandler<LoginUserCommandRequest, ResultDto<string?>>
{
    private readonly IIdentityService _identityService;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;
    public LoginUserCommandRequestHandler(IIdentityService identityService, IJwtProvider jwtProvider, IMapper mapper)
    {
        _identityService = identityService;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
    }

    public async Task<ResultDto<string?>> Handle(LoginUserCommandRequest command, CancellationToken cancellationToken)
    {
        Result<User?> userResult = await _identityService.AuthenticateMember(command.UserName, command.Password);

        return _mapper.Map<ResultDto<string?>>(
            userResult.IsSuccess()
            ? _jwtProvider.Generate(userResult.GetValue())
            : Result.Failure<string>(userResult.GetError())
        );
    }
}
