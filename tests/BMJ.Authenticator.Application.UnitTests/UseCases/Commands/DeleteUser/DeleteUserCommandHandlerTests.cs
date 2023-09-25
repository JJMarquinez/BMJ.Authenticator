using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Errors.Builders;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Results.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser;
using MediatR;
using Moq;

namespace BMJ.Authenticator.Application.UnitTests.UseCases.Commands.DeleteUser;

public class DeleteUserCommandHandlerTests
{
    private readonly Mock<IIdentityAdapter> _identityAdapter;
    private readonly IResultDtoBuilder _resultDtoBuilder;
    private readonly IErrorDtoBuilder _errorDtoBuilder;

    public DeleteUserCommandHandlerTests()
    {
        _identityAdapter = new();
        _resultDtoBuilder = new ResultDtoBuilder();
        _errorDtoBuilder = new ErrorDtoBuilder();
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
            )).ReturnsAsync(_resultDtoBuilder.BuildSuccess());
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
            )).ReturnsAsync(_resultDtoBuilder.WithError(_errorDtoBuilder.Build()).Build());
        IRequestHandler<DeleteUserCommand, ResultDto> handler = new DeleteUserCommandHandler(_identityAdapter.Object);

        var resultDto = await handler.Handle(command, token);

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
    }
}
