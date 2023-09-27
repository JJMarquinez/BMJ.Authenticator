using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Errors.Builders;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Results.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser;
using BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser.Builders;
using BMJ.Authenticator.Domain.Entities.Users;
using MediatR;
using Moq;

namespace BMJ.Authenticator.Application.UnitTests.UseCases.Commands.DeleteUser;

public class DeleteUserCommandHandlerTests
{
    private readonly Mock<IIdentityAdapter> _identityAdapter;
    private readonly IResultDtoBuilder _resultDtoBuilder;
    private readonly IErrorDtoBuilder _errorDtoBuilder;
    private readonly IDeleteUserCommandBuilder _deleteUserCommandBuilder;

    public DeleteUserCommandHandlerTests()
    {
        _identityAdapter = new();
        _resultDtoBuilder = new ResultDtoBuilder();
        _errorDtoBuilder = new ErrorDtoBuilder();
        _deleteUserCommandBuilder = new DeleteUserCommandBuilder();
    }

    [Fact]
    public async void ShouldDeleteUser()
    {
        var command = _deleteUserCommandBuilder.WithId(Guid.NewGuid().ToString()).Build();
        var token = new CancellationTokenSource().Token;
        _identityAdapter.Setup(x => x.DeleteUserAsync(
            It.IsAny<string>()
            )).ReturnsAsync(_resultDtoBuilder.BuildSuccess());
        IRequestHandler<DeleteUserCommand, ResultDto> handler = new DeleteUserCommandHandler(_identityAdapter.Object);

        var resultDto = await handler.Handle((DeleteUserCommand)command, token);

        Assert.NotNull(resultDto);
        Assert.True(resultDto.Success);
    }

    [Fact]
    public async void ShouldNotDeleteUser()
    {
        var command = _deleteUserCommandBuilder.WithId(Guid.NewGuid().ToString()).Build();
        var token = new CancellationTokenSource().Token;
        _identityAdapter.Setup(x => x.DeleteUserAsync(
            It.IsAny<string>()
            )).ReturnsAsync(_resultDtoBuilder.WithError(_errorDtoBuilder.Build()).Build());
        IRequestHandler<DeleteUserCommand, ResultDto> handler = new DeleteUserCommandHandler(_identityAdapter.Object);

        var resultDto = await handler.Handle((DeleteUserCommand)command, token);

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
    }
}
