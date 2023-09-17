using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.LoginUser;

public class LoginUserQueryHandler
    : IRequestHandler<LoginUserQuery, ResultDto<string?>>
{
    private readonly IIdentityAdapter _identityAdapter;
    private readonly IJwtProvider _jwtProvider;
    public LoginUserQueryHandler(IIdentityAdapter identityAdapter, IJwtProvider jwtProvider)
    {
        _identityAdapter = identityAdapter;
        _jwtProvider = jwtProvider;
    }

    public async Task<ResultDto<string?>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var userResultDto = await _identityAdapter.AuthenticateMemberAsync(request.UserName!, request.Password!);
        ResultDto<string?> response;

        if (userResultDto.Success)
        {
            response = ResultDto<string?>.NewSuccess<string?>(await _jwtProvider.GenerateAsync(userResultDto.Value!));
        }
        else
            response = ResultDto<string?>.NewFailure<string?>(userResultDto.Error);

        return response;
    }
}
