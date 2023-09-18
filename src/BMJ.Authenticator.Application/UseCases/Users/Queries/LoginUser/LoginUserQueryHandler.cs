using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Results.FactoryMethods;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.LoginUser;

public class LoginUserQueryHandler
    : IRequestHandler<LoginUserQuery, ResultDto<string?>>
{
    private readonly IIdentityAdapter _identityAdapter;
    private readonly IJwtProvider _jwtProvider;
    private readonly IResultDtoCreator _resultDtoCreator;
    public LoginUserQueryHandler(IIdentityAdapter identityAdapter, IJwtProvider jwtProvider, IResultDtoCreator resultDtoCreator)
    {
        _identityAdapter = identityAdapter;
        _jwtProvider = jwtProvider;
        _resultDtoCreator = resultDtoCreator;
    }

    public async Task<ResultDto<string?>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var userResultDto = await _identityAdapter.AuthenticateMemberAsync(request.UserName!, request.Password!);
        ResultDto<string?> response;

        if (userResultDto.Success)
        {
            response = _resultDtoCreator.CreateSuccessResult<string?>(await _jwtProvider.GenerateAsync(userResultDto.Value!));
        }
        else
            response = _resultDtoCreator.CreateFailureResult<string?>(userResultDto.Error);

        return response;
    }
}
