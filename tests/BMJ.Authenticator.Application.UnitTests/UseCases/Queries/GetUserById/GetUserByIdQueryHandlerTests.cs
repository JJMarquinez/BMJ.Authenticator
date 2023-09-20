using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Results.FactoryMethods;
using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Application.Common.Models.Users.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById;
using BMJ.Authenticator.Domain.ValueObjects;
using MediatR;
using Moq;
using System.Xml.Linq;

namespace BMJ.Authenticator.Application.UnitTests.UseCases.Queries.GetUserById;

public class GetUserByIdQueryHandlerTests
{
    private readonly Mock<IIdentityAdapter> _identityAdapter;
    private readonly IResultDtoCreator _resultDtoCreator;
    private readonly IUserDtoBuilder _userDtoBuilder;

    public GetUserByIdQueryHandlerTests()
    {
        _identityAdapter = new();
        _resultDtoCreator = new ResultDtoCreator(new ResultDtoFactory(), new ResultDtoGenericFactory());
        _userDtoBuilder = new UserDtoBuilder();
    }

    [Fact]
    public async void ShouldGetUserById()
    {
        var query = new GetUserByIdQuery
        { 
            Id = Guid.NewGuid().ToString(),
        };
        var token = new CancellationTokenSource().Token;
        var user = _userDtoBuilder
            .WithId(Guid.NewGuid().ToString())
            .WithName("Paola")
            .WithEmail("paola@auth.com")
            .WithPhone("222-555-888")
            .Build();
        _identityAdapter.Setup(x => x.GetUserByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(_resultDtoCreator.CreateSuccessResult<UserDto?>(user));
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
            )).ReturnsAsync(_resultDtoCreator.CreateFailureResult<UserDto?>(new Application.Common.Models.ErrorDto()));
        IRequestHandler<GetUserByIdQuery, ResultDto<UserDto?>> handler = new GetUserByIdQueryHandler(_identityAdapter.Object);

        var resultDto = await handler.Handle(query, token);

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
    }
}
