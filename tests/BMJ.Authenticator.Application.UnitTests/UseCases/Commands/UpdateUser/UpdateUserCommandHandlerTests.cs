using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser;
using MediatR;
using Moq;

namespace BMJ.Authenticator.Application.UnitTests.UseCases.Commands.DeleteUser;

public class UpdateUserCommandHandlerTests
{
    private readonly Mock<IIdentityAdapter> _identityAdapter;

    public UpdateUserCommandHandlerTests()
    {
        _identityAdapter = new();
    }

    [Fact]
    public async void ShouldUpdateUser()
    {
        var command = new UpdateUserCommand
        {
            UserName = "davis",
            Email = "davis@auth.com",
            PhoneNumber = "999-888-777"
        };
        var token = new CancellationTokenSource().Token;
        _identityAdapter.Setup(x => x.UpdateUserAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()
            )).ReturnsAsync(ResultDto.NewSuccess());
        IRequestHandler<UpdateUserCommand, ResultDto> handler = new UpdateUserCommandHandler(_identityAdapter.Object);

        var resultDto = await handler.Handle(command, token);

        Assert.NotNull(resultDto);
        Assert.True(resultDto.Success);
    }

    [Fact]
    public async void ShouldNotUpdateUser()
    {
        var command = new UpdateUserCommand
        {
            UserName = "davis",
            Email = "davis@auth.com",
            PhoneNumber = "999-888-777"
        };
        var token = new CancellationTokenSource().Token;
        _identityAdapter.Setup(x => x.UpdateUserAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()
            )).ReturnsAsync(ResultDto.NewFailure(new Application.Common.Models.ErrorDto()));
        IRequestHandler<UpdateUserCommand, ResultDto> handler = new UpdateUserCommandHandler(_identityAdapter.Object);

        var resultDto = await handler.Handle(command, token);

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
    }
}
