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
        IRequestHandler<UpdateUserCommand, ResultDto> handler = new UpdateUserCommandHandler(_identityAdapter.Object);

        var resultDto = handler.Handle(command, token);

        Assert.NotNull(resultDto);
    }
}
