using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Results.Builders;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.LoginUser;

public class LoginUserQueryHandler
    : IRequestHandler<LoginUserQuery, ResultDto<string?>>
{
    private readonly IIdentityAdapter _identityAdapter;
    private readonly IJwtProvider _jwtProvider;
    private readonly IResultDtoGenericBuilder _resultDtoGenericBuilder;
    public LoginUserQueryHandler(IIdentityAdapter identityAdapter, IJwtProvider jwtProvider, IResultDtoGenericBuilder resultDtoGenericBuilder)
    {
        _identityAdapter = identityAdapter;
        _jwtProvider = jwtProvider;
        _resultDtoGenericBuilder = resultDtoGenericBuilder;
    }

    public async Task<ResultDto<string?>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var userResultDto = await _identityAdapter.AuthenticateMemberAsync(request.UserName!, request.Password!);
        ResultDto<string?> response;

        if (userResultDto.Success)
        {
            response = _resultDtoGenericBuilder.BuildSuccess<string?>(await _jwtProvider.GenerateAsync(userResultDto.Value!));
        }
        else
            response = _resultDtoGenericBuilder.BuildFailure<string?>(userResultDto.Error);

        return response;
    }
}
