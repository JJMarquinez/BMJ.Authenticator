using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser;
using MediatR;
using Moq;

namespace BMJ.Authenticator.Application.UnitTests.UseCases.Commands.DeleteUser;

public class DeleteUserCommandHandlerTests
{
    private readonly Mock<IIdentityAdapter> _identityAdapter;

    public DeleteUserCommandHandlerTests()
    {
        _identityAdapter = new();
    }

    [Fact]
    public async void ShouldDeleteUser()
    {
        var command = new DeleteUserCommand
        {
            Id = Guid.NewGuid().ToString(),
        };
        var token = new CancellationTokenSource().Token;
        _identityAdapter.Setup(x => x.DeleteUserAsync(
            It.IsAny<string>()
            )).ReturnsAsync(ResultDto.NewSuccess());
        IRequestHandler<DeleteUserCommand, ResultDto> handler = new DeleteUserCommandHandler(_identityAdapter.Object);

        var resultDto = await handler.Handle(command, token);

        Assert.NotNull(resultDto);
        Assert.True(resultDto.Success);
    }

    [Fact]
    public async void ShouldNotDeleteUser()
    {
        var command = new DeleteUserCommand
        {
            Id = Guid.NewGuid().ToString(),
        };
        var token = new CancellationTokenSource().Token;
        _identityAdapter.Setup(x => x.DeleteUserAsync(
            It.IsAny<string>()
            )).ReturnsAsync(ResultDto.NewFailure(new Common.Models.ErrorDto()));
        IRequestHandler<DeleteUserCommand, ResultDto> handler = new DeleteUserCommandHandler(_identityAdapter.Object);

        var resultDto = await handler.Handle(command, token);

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
    }
}
