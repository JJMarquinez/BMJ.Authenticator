using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Errors.Builders;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Results.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser;
using BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser.Builders;
using MediatR;
using Moq;

namespace BMJ.Authenticator.Application.UnitTests.UseCases.Commands.CreateUser;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IIdentityAdapter> _identityAdapter;
    private readonly IErrorDtoBuilder _errorDtoBuilder;
    private readonly IResultDtoBuilder _resultDtoBuilder;
    private readonly ICreateUserCommandBuilder _createUserCommandBuilder;

    public CreateUserCommandHandlerTests()
    {
        _identityAdapter = new();
        _resultDtoBuilder = new ResultDtoBuilder();
        _errorDtoBuilder = new ErrorDtoBuilder();
        _createUserCommandBuilder = new CreateUserCommandBuilder();
    }

    [Fact]
    public async void ShouldCreateUser()
    {
        var command = _createUserCommandBuilder
            .WithUsername("davis")
            .WithEmail("davis@auth.com")
            .WithPhoneNumber("999-888-777")
            .WithPassword("@3cT6uxn$KR6")
            .Build();
        var token = new CancellationTokenSource().Token;
        _identityAdapter.Setup(x => x.CreateUserAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string?>()
            )).ReturnsAsync(_resultDtoBuilder.BuildSuccess());
        IRequestHandler<CreateUserCommand, ResultDto> handler = new CreateUserCommandHandler(_identityAdapter.Object);

        var resultDto = await handler.Handle((CreateUserCommand)command, token);

        Assert.NotNull(resultDto);
        Assert.True(resultDto.Success);
    }

    [Fact]
    public async void ShouldNotCreateUser()
    {
        var command = _createUserCommandBuilder
            .WithUsername("davis")
            .WithEmail("davis@auth.com")
            .WithPhoneNumber("999-888-777")
            .WithPassword("@3cT6uxn$KR6")
            .Build();
        var token = new CancellationTokenSource().Token;
        _identityAdapter.Setup(x => x.CreateUserAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string?>()
            )).ReturnsAsync(_resultDtoBuilder.WithError(_errorDtoBuilder.Build()).Build());
        IRequestHandler<CreateUserCommand, ResultDto> handler = new CreateUserCommandHandler(_identityAdapter.Object);

        var resultDto = await handler.Handle((CreateUserCommand)command, token);

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
    }
}
