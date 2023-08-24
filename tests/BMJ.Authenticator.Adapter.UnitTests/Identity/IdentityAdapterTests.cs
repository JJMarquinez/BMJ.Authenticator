using BMJ.Authenticator.Adapter.Common;
using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Adapter.Identity;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models;
using BMJ.Authenticator.Application.Common.Models.Results;
using Moq;
using System.Text.Json;

namespace BMJ.Authenticator.Adapter.UnitTests.Identity;

public class IdentityAdapterTests
{
    private readonly Mock<IIdentityService> _identityService;
    private readonly Mock<IAuthLogger> _logger;
    private readonly UserIdentification _user;
    private readonly string _userJson;

    public IdentityAdapterTests()
    {
        _identityService = new();
        _logger = new();
        _user = new UserIdentification()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Jame",
            Email = "jame@auth.com",
            PhoneNumber = "111-222-3333",
            Roles = new[] { "Standard" }
        };
        _userJson = JsonSerializer.Serialize(_user);
    }

    [Fact]
    public async void ShouldAuthenticateUser()
    {
        _identityService.Setup(x => x.AuthenticateMemberAsync(
            It.IsAny<string>(),
            It.IsAny<string>()
            )).ReturnsAsync(ResultDto<string?>.NewSuccess<string?>(_userJson));
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object);

        var resultDto = await identityAdapter.AuthenticateMemberAsync("Jame", "oT586n@S&#nJ");
        
        Assert.NotNull(resultDto);
        Assert.True(resultDto.Success);
        Assert.Equal(resultDto.Value!.Id, _user.Id);
        Assert.Equal(resultDto.Value!.UserName, _user.UserName);
        Assert.Equal(resultDto.Value!.Email, _user.Email);
        Assert.Equal(resultDto.Value!.PhoneNumber, _user.PhoneNumber);
        Assert.Equal(resultDto.Value!.Roles, _user.Roles);
    }

    [Fact]
    public async void ShouldNotAuthenticateUser()
    {
        var error = InfrastructureError.Identity.UserNameOrPasswordNotValid;
        _identityService.Setup(x => x.AuthenticateMemberAsync(
            It.IsAny<string>(),
            It.IsAny<string>()
            )).ReturnsAsync(ResultDto<string?>.NewFailure<string?>(error));
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object);

        var resultDto = await identityAdapter.AuthenticateMemberAsync("Jame", "oT586n@S&#nJ");

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
        Assert.Equal(resultDto.Error.Title, error.Title);
        Assert.Equal(resultDto.Error.Code, error.Code);
        Assert.Equal(resultDto.Error.Detail, error.Detail);
        Assert.Equal(resultDto.Error.HttpStatusCode, error.HttpStatusCode);
    }

    [Fact]
    public async void ShouldCreateUser()
    {
        _identityService.Setup(x => x.CreateUserAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string?>()
            )).ReturnsAsync(ResultDto.NewSuccess());
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object);

        var resultDto = await identityAdapter.CreateUserAsync("Jame", "oT586n@S&#nJ", "jame@auth.com", "111-222-3333");

        Assert.NotNull(resultDto);
        Assert.True(resultDto.Success);
    }

    [Fact]
    public async void ShouldNotCreateUser()
    {
        var error = InfrastructureError.Identity.UserWasNotCreated;
        _identityService.Setup(x => x.CreateUserAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string?>()
            )).ReturnsAsync(ResultDto.NewFailure(error));
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object);

        var resultDto = await identityAdapter.CreateUserAsync("Jame", "oT586n@S&#nJ", "jame@auth.com", "111-222-3333");

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
        Assert.Equal(resultDto.Error.Title, error.Title);
        Assert.Equal(resultDto.Error.Code, error.Code);
        Assert.Equal(resultDto.Error.Detail, error.Detail);
        Assert.Equal(resultDto.Error.HttpStatusCode, error.HttpStatusCode);
        _logger.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<ErrorDto>()), Times.Once);
    }

    [Fact]
    public async void ShouldDeleteUser()
    {
        _identityService.Setup(x => x.DeleteUserAsync(
            It.IsAny<string>()
            )).ReturnsAsync(ResultDto.NewSuccess());
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object);

        var resultDto = await identityAdapter.DeleteUserAsync(Guid.NewGuid().ToString());

        Assert.NotNull(resultDto);
        Assert.True(resultDto.Success);
    }

    [Fact]
    public async void ShouldNotDeleteUser()
    {
        var error = InfrastructureError.Identity.UserWasNotDeleted;
        _identityService.Setup(x => x.DeleteUserAsync(
            It.IsAny<string>()
            )).ReturnsAsync(ResultDto.NewFailure(error));
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object);

        var resultDto = await identityAdapter.DeleteUserAsync(Guid.NewGuid().ToString());

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
        Assert.Equal(resultDto.Error.Title, error.Title);
        Assert.Equal(resultDto.Error.Code, error.Code);
        Assert.Equal(resultDto.Error.Detail, error.Detail);
        Assert.Equal(resultDto.Error.HttpStatusCode, error.HttpStatusCode);
        _logger.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<ErrorDto>()), Times.Once);
    }
}
