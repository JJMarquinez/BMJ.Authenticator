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
        IRequestHandler<DeleteUserCommand, ResultDto> handler = new DeleteUserCommandHandler(_identityAdapter.Object);

        var resultDto = handler.Handle(command, token);

        Assert.NotNull(resultDto);
    }
}
