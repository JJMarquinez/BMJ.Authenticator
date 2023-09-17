using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Application.UseCases.Users.Queries.LoginUser;
using MediatR;
using Moq;

namespace BMJ.Authenticator.Application.UnitTests.UseCases.Queries.LoginUser;

public class LoginUserQueryHandlerTests
{
    private readonly Mock<IIdentityAdapter> _identityAdapter;
    private readonly Mock<IJwtProvider> _jwtProvider;
    private readonly LoginUserQuery _query;
    public LoginUserQueryHandlerTests()
    {
        _identityAdapter = new();
        _jwtProvider = new();
        _query = new LoginUserQuery
        {
            UserName = "Dan",
            Password = "O8p1w1aI0&c@"
        };
    }

    [Fact]
    public async void ShouldLoginUserReturningToken()
    {
        var token = new CancellationTokenSource().Token;
        var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNThhMDRjZS1hZmI1LTRjYzktOGI1Ni1kOWEzMWI1MWZjMGIiLCJuYW1lIjoiYWRtaW4iLCJlbWFpbCI6ImFkbWluaXN0cmF0b3JAbG9jYWxob3N0LmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluaXN0cmF0b3IiLCJleHAiOjE2OTI3ODgxMTIsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3QifQ.Ih0cs_yQDVLy90pHpFSZY5z1_16Or2x82pySKnejfgg";
        var user = new UserDto() { Id = Guid.NewGuid().ToString(), UserName = "Dan", Email = "dan@auth.com", PhoneNumber = "666-555-444" };
        _identityAdapter.Setup(x => x.AuthenticateMemberAsync(
            It.IsAny<string>(),
            It.IsAny<string>()
            )).ReturnsAsync(ResultDto<UserDto?>.NewSuccess<UserDto?>(user));
        _jwtProvider.Setup(x => x.GenerateAsync(It.IsAny<UserDto>())).ReturnsAsync(jwtToken);
        IRequestHandler<LoginUserQuery, ResultDto<string?>> handler = new LoginUserQueryHandler(_identityAdapter.Object, _jwtProvider.Object);

        var resultDto = await handler.Handle(_query, token);

        Assert.NotNull(resultDto);
        Assert.True(resultDto.Success);
        Assert.Equal(jwtToken, resultDto.Value!);
    }

    [Fact]
    public async void ShouldNotLoginUser()
    {
        var token = new CancellationTokenSource().Token;
        var error = new ErrorDto
        {
            Code = "Identity.Argument.Test.UserNameOrPasswordNotValid",
            Title = "User name or password aren't valid.",
            Detail = "The user name or password wich were sent are not correct, either the user doesn't exist or password isn't correct.",
            HttpStatusCode = 409
        };
        _identityAdapter.Setup(x => x.AuthenticateMemberAsync(
            It.IsAny<string>(),
            It.IsAny<string>()
            )).ReturnsAsync(ResultDto<UserDto?>.NewFailure<UserDto?>(error));
        IRequestHandler<LoginUserQuery, ResultDto<string?>> handler = new LoginUserQueryHandler(_identityAdapter.Object, _jwtProvider.Object);

        var resultDto = await handler.Handle(_query, token);

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
        Assert.Equal(resultDto.Error.Title, error.Title);
        Assert.Equal(resultDto.Error.Code, error.Code);
        Assert.Equal(resultDto.Error.Detail, error.Detail);
        Assert.Equal(resultDto.Error.HttpStatusCode, error.HttpStatusCode);
    }
}
