using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Errors.Builders;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Results.FactoryMethods;
using BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser;
using MediatR;
using Moq;

namespace BMJ.Authenticator.Application.UnitTests.UseCases.Commands.DeleteUser;

public class DeleteUserCommandHandlerTests
{
    private readonly Mock<IIdentityAdapter> _identityAdapter;
    private readonly IResultDtoCreator _resultDtoCreator;
    private readonly IErrorDtoBuilder _errorDtoBuilder;

    public DeleteUserCommandHandlerTests()
    {
        _identityAdapter = new();
        _resultDtoCreator = new ResultDtoCreator(new ResultDtoFactory(), new ResultDtoGenericFactory());
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
            )).ReturnsAsync(_resultDtoCreator.CreateSuccessResult());
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
            )).ReturnsAsync(_resultDtoCreator.CreateFailureResult(_errorDtoBuilder.Build()));
        IRequestHandler<DeleteUserCommand, ResultDto> handler = new DeleteUserCommandHandler(_identityAdapter.Object);

        var resultDto = await handler.Handle(command, token);

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
    }
}
