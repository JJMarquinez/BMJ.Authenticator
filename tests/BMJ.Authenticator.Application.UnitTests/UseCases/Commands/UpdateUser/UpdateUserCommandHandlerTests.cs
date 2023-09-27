using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Errors.Builders;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Results.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser;
using BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser.Builders;
using MediatR;
using Moq;

namespace BMJ.Authenticator.Application.UnitTests.UseCases.Commands.DeleteUser;

public class UpdateUserCommandHandlerTests
{
    private readonly Mock<IIdentityAdapter> _identityAdapter;
    private readonly IResultDtoBuilder _resultDtoBuilder;
    private readonly IErrorDtoBuilder _errorDtoBuilder;
    private readonly IUpdateUserCommandBuilder _updateUserCommandBuilder;

    public UpdateUserCommandHandlerTests()
    {
        _identityAdapter = new();
        _resultDtoBuilder = new ResultDtoBuilder();
        _errorDtoBuilder = new ErrorDtoBuilder();
        _updateUserCommandBuilder = new UpdateUserCommandBuilder();
    }

    [Fact]
    public async void ShouldUpdateUser()
    {
        var command = _updateUserCommandBuilder
            .WithUsername("davis")
            .WithEmail("davis@auth.com")
            .WithPhoneNumber("999-888-777")
            .Build();
        var token = new CancellationTokenSource().Token;
        _identityAdapter.Setup(x => x.UpdateUserAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()
            )).ReturnsAsync(_resultDtoBuilder.BuildSuccess());
        IRequestHandler<UpdateUserCommand, ResultDto> handler = new UpdateUserCommandHandler(_identityAdapter.Object);

        var resultDto = await handler.Handle((UpdateUserCommand)command, token);

        Assert.NotNull(resultDto);
        Assert.True(resultDto.Success);
    }

    [Fact]
    public async void ShouldNotUpdateUser()
    {
        var command = _updateUserCommandBuilder
            .WithUsername("davis")
            .WithEmail("davis@auth.com")
            .WithPhoneNumber("999-888-777")
            .Build();
        var token = new CancellationTokenSource().Token;
        _identityAdapter.Setup(x => x.UpdateUserAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()
            )).ReturnsAsync(_resultDtoBuilder.WithError(_errorDtoBuilder.Build()).Build());
        IRequestHandler<UpdateUserCommand, ResultDto> handler = new UpdateUserCommandHandler(_identityAdapter.Object);

        var resultDto = await handler.Handle((UpdateUserCommand)command, token);

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
    }
}
