using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser;
using MediatR;
using Moq;

namespace BMJ.Authenticator.Application.UnitTests.UseCases.Commands.CreateUser;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IIdentityAdapter> _identityAdapter;
    public CreateUserCommandHandlerTests()
    {
        _identityAdapter = new();
    }

    [Fact]
    public async void ShouldCreateUser()
    {
        var command = new CreateUserCommand
        {
            UserName = "davis",
            Email = "davis@auth.com",
            PhoneNumber = "999-888-777",
            Password = "@3cT6uxn$KR6"
        };
        var token = new CancellationTokenSource().Token;
        IRequestHandler<CreateUserCommand, ResultDto> handler = new CreateUserCommandHandler(_identityAdapter.Object);

        var resultDto = handler.Handle(command, token);

        Assert.NotNull(resultDto);
    }
}
