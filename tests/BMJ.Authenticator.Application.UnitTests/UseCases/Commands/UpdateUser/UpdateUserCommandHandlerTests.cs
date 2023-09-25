using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Errors.Builders;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Results.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser;
using MediatR;
using Moq;

namespace BMJ.Authenticator.Application.UnitTests.UseCases.Commands.DeleteUser;

public class UpdateUserCommandHandlerTests
{
    private readonly Mock<IIdentityAdapter> _identityAdapter;
    private readonly IResultDtoBuilder _resultDtoBuilder;
    private readonly IErrorDtoBuilder _errorDtoBuilder;

    public UpdateUserCommandHandlerTests()
    {
        _identityAdapter = new();
        _resultDtoBuilder = new ResultDtoBuilder();
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
            )).ReturnsAsync(_resultDtoBuilder.BuildSuccess());
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
            )).ReturnsAsync(_resultDtoBuilder.WithError(_errorDtoBuilder.Build()).Build());
        IRequestHandler<UpdateUserCommand, ResultDto> handler = new UpdateUserCommandHandler(_identityAdapter.Object);

        var resultDto = await handler.Handle(command, token);

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
    }
}
