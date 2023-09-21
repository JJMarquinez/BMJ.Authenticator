using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Errors.Builders;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Results.FactoryMethods;
using BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser;
using MediatR;
using Moq;

namespace BMJ.Authenticator.Application.UnitTests.UseCases.Commands.DeleteUser;

public class UpdateUserCommandHandlerTests
{
    private readonly Mock<IIdentityAdapter> _identityAdapter;
    private readonly IResultDtoCreator _resultDtoCreator;
    private readonly IErrorDtoBuilder _errorDtoBuilder;

    public UpdateUserCommandHandlerTests()
    {
        _identityAdapter = new();
        _resultDtoCreator = new ResultDtoCreator(new ResultDtoFactory(), new ResultDtoGenericFactory());
        _errorDtoBuilder = new ErrorDtoBuilder();
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
            )).ReturnsAsync(_resultDtoCreator.CreateSuccessResult());
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
            )).ReturnsAsync(_resultDtoCreator.CreateFailureResult(_errorDtoBuilder.Build()));
        IRequestHandler<UpdateUserCommand, ResultDto> handler = new UpdateUserCommandHandler(_identityAdapter.Object);

        var resultDto = await handler.Handle(command, token);

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
    }
}
