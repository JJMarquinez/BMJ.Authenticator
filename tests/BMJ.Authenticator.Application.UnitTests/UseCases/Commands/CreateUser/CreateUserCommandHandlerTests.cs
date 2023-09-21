using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Errors.Builders;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Results.FactoryMethods;
using BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser;
using MediatR;
using Moq;

namespace BMJ.Authenticator.Application.UnitTests.UseCases.Commands.CreateUser;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IIdentityAdapter> _identityAdapter;
    private readonly IResultDtoCreator _resultDtoCreator;
    private readonly IErrorDtoBuilder _errorDtoBuilder;

    public CreateUserCommandHandlerTests()
    {
        _identityAdapter = new();
        _resultDtoCreator = new ResultDtoCreator(new ResultDtoFactory(), new ResultDtoGenericFactory());
        _errorDtoBuilder = new ErrorDtoBuilder();
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
        _identityAdapter.Setup(x => x.CreateUserAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string?>()
            )).ReturnsAsync(_resultDtoCreator.CreateSuccessResult());
        IRequestHandler<CreateUserCommand, ResultDto> handler = new CreateUserCommandHandler(_identityAdapter.Object);

        var resultDto = await handler.Handle(command, token);

        Assert.NotNull(resultDto);
        Assert.True(resultDto.Success);
    }

    [Fact]
    public async void ShouldNotCreateUser()
    {
        var command = new CreateUserCommand
        {
            UserName = "davis",
            Email = "davis@auth.com",
            PhoneNumber = "999-888-777",
            Password = "@3cT6uxn$KR6"
        };
        var token = new CancellationTokenSource().Token;
        _identityAdapter.Setup(x => x.CreateUserAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string?>()
            )).ReturnsAsync(_resultDtoCreator.CreateFailureResult(_errorDtoBuilder.Build()));
        IRequestHandler<CreateUserCommand, ResultDto> handler = new CreateUserCommandHandler(_identityAdapter.Object);

        var resultDto = await handler.Handle(command, token);

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
    }
}
