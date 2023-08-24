using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById;
using MediatR;
using Moq;
using System.Xml.Linq;

namespace BMJ.Authenticator.Application.UnitTests.UseCases.Queries.GetUserById;

public class GetUserByIdQueryHandlerTests
{
    private readonly Mock<IIdentityAdapter> _identityAdapter;
    public GetUserByIdQueryHandlerTests()
    {
        _identityAdapter = new();
    }

    [Fact]
    public async void ShouldGetUserById()
    {
        var query = new GetUserByIdQuery
        { 
            Id = Guid.NewGuid().ToString(),
        };
        var token = new CancellationTokenSource().Token;
        var user = new UserDto
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Paola",
            Email = "paola@auth.com",
            PhoneNumber = "222-555-888"
        };
        _identityAdapter.Setup(x => x.GetUserByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(ResultDto<UserDto?>.NewSuccess<UserDto?>(user));
        IRequestHandler<GetUserByIdQuery, ResultDto<UserDto?>> handler = new GetUserByIdQueryHandler(_identityAdapter.Object);

        var resultDto = await handler.Handle(query, token);

        Assert.NotNull(resultDto);
        Assert.True(resultDto.Success);
        Assert.Equal(user.Id, resultDto.Value!.Id);
        Assert.Equal(user.UserName, resultDto.Value!.UserName);
        Assert.Equal(user.Email, resultDto.Value!.Email);
        Assert.Equal(user.PhoneNumber, resultDto.Value!.PhoneNumber);
        Assert.Equal(user.Roles, resultDto.Value!.Roles);
    }

    [Fact]
    public async void ShouldNotGetUserById()
    {
        var query = new GetUserByIdQuery
        {
            Id = Guid.NewGuid().ToString(),
        };
        var token = new CancellationTokenSource().Token;
        _identityAdapter.Setup(x => x.GetUserByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(ResultDto<UserDto?>.NewFailure<UserDto?>(new Common.Models.ErrorDto()));
        IRequestHandler<GetUserByIdQuery, ResultDto<UserDto?>> handler = new GetUserByIdQueryHandler(_identityAdapter.Object);

        var resultDto = await handler.Handle(query, token);

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
    }
}
