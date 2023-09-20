using BMJ.Authenticator.Adapter.Common;
using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Adapter.Identity;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models;
using BMJ.Authenticator.Application.Common.Models.Results.FactoryMethods;
using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Application.Common.Models.Users.Builders;
using Moq;
using System.Text.Json;

namespace BMJ.Authenticator.Adapter.UnitTests.Identity;

public class IdentityAdapterTests
{
    private readonly Mock<IIdentityService> _identityService;
    private readonly Mock<IAuthLogger> _logger;
    private readonly UserIdentification _jame, _penelope;
    private readonly IResultDtoCreator _resultDtoCreator;
    private readonly IUserDtoBuilder _userDtoBuilder;

    public IdentityAdapterTests()
    {
        _identityService = new();
        _logger = new();
        _resultDtoCreator = new ResultDtoCreator(new ResultDtoFactory(), new ResultDtoGenericFactory());
        _userDtoBuilder = new UserDtoBuilder();
        _jame = new UserIdentification
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Jame",
            Email = "jame@auth.com",
            PhoneNumber = "111-222-3333",
            Roles = new[] { "Standard" }
        };
        _penelope = new UserIdentification
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Penelope",
            Email = "penelope@auth.com",
            PhoneNumber = "444-222-3333",
            Roles = new[] { "Administrator" }
        };
    }

    [Fact]
    public async void ShouldAuthenticateUser()
    {
        var userJson = JsonSerializer.Serialize(_jame);
        _identityService.Setup(x => x.AuthenticateMemberAsync(
            It.IsAny<string>(),
            It.IsAny<string>()
            )).ReturnsAsync(_resultDtoCreator.CreateSuccessResult<string?>(userJson));
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object, _resultDtoCreator);

        var resultDto = await identityAdapter.AuthenticateMemberAsync("Jame", "oT586n@S&#nJ");
        
        Assert.NotNull(resultDto);
        Assert.True(resultDto.Success);
        Assert.Equal(resultDto.Value!.Id, _jame.Id);
        Assert.Equal(resultDto.Value!.UserName, _jame.UserName);
        Assert.Equal(resultDto.Value!.Email, _jame.Email);
        Assert.Equal(resultDto.Value!.PhoneNumber, _jame.PhoneNumber);
        Assert.Equal(resultDto.Value!.Roles, _jame.Roles);
    }

    [Fact]
    public async void ShouldNotAuthenticateUser()
    {
        var error = InfrastructureError.Identity.UserNameOrPasswordNotValid;
        _identityService.Setup(x => x.AuthenticateMemberAsync(
            It.IsAny<string>(),
            It.IsAny<string>()
            )).ReturnsAsync(_resultDtoCreator.CreateFailureResult<string?>(error));
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object, _resultDtoCreator);

        var resultDto = await identityAdapter.AuthenticateMemberAsync("Jame", "oT586n@S&#nJ");

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
        Assert.Equal(error.Title, resultDto.Error.Title);
        Assert.Equal(error.Code, resultDto.Error.Code);
        Assert.Equal(error.Detail, resultDto.Error.Detail);
        Assert.Equal(error.HttpStatusCode, resultDto.Error.HttpStatusCode);
    }

    [Fact]
    public async void ShouldCreateUser()
    {
        _identityService.Setup(x => x.CreateUserAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string?>()
            )).ReturnsAsync(_resultDtoCreator.CreateSuccessResult());
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object, _resultDtoCreator);

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
            )).ReturnsAsync(_resultDtoCreator.CreateFailureResult(error));
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object, _resultDtoCreator);

        var resultDto = await identityAdapter.CreateUserAsync("Jame", "oT586n@S&#nJ", "jame@auth.com", "111-222-3333");

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
        Assert.Equal(error.Title, resultDto.Error.Title);
        Assert.Equal(error.Code, resultDto.Error.Code);
        Assert.Equal(error.Detail, resultDto.Error.Detail);
        Assert.Equal(error.HttpStatusCode, resultDto.Error.HttpStatusCode);
        _logger.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<ErrorDto>()), Times.Once);
    }

    [Fact]
    public async void ShouldDeleteUser()
    {
        _identityService.Setup(x => x.DeleteUserAsync(
            It.IsAny<string>()
            )).ReturnsAsync(_resultDtoCreator.CreateSuccessResult());
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object, _resultDtoCreator);

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
            )).ReturnsAsync(_resultDtoCreator.CreateFailureResult(error));
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object, _resultDtoCreator);

        var resultDto = await identityAdapter.DeleteUserAsync(Guid.NewGuid().ToString());

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
        Assert.Equal(error.Title, resultDto.Error.Title);
        Assert.Equal(error.Code, resultDto.Error.Code);
        Assert.Equal(error.Detail, resultDto.Error.Detail);
        Assert.Equal(error.HttpStatusCode, resultDto.Error.HttpStatusCode);
        _logger.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<ErrorDto>()), Times.Once);
    }

    [Fact]
    public void ShouldSayUsernameDoesNotExist()
    {
        _identityService.Setup(x => x.DoesUserNameNotExist(
            It.IsAny<string>()
            )).Returns(true);
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object, _resultDtoCreator);

        var notExist = identityAdapter.DoesUserNameNotExist("Jaime");

        Assert.True(notExist);
    }

    [Fact]
    public void ShouldSayUsernameExists()
    {
        _identityService.Setup(x => x.DoesUserNameNotExist(
            It.IsAny<string>()
            )).Returns(false);
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object, _resultDtoCreator);

        var notExist = identityAdapter.DoesUserNameNotExist("Jaime");

        Assert.False(notExist);
    }

    [Fact]
    public async void ShouldGetAllUsers()
    {
        var listUsersJson = JsonSerializer.Serialize(new List<UserIdentification> { _penelope, _jame });
        _identityService.Setup(x => x.GetAllUserAsync()).ReturnsAsync(_resultDtoCreator.CreateSuccessResult<string?>(listUsersJson));
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object, _resultDtoCreator);
        var userDtoList = new List<UserDto>
        {
            _userDtoBuilder
            .WithId(_penelope.Id)
            .WithName(_penelope.UserName)
            .WithEmail(_penelope.Email)
            .WithPhone(_penelope.PhoneNumber)
            .WithRoles(_penelope.Roles)
            .Build(),
            _userDtoBuilder
            .WithId(_jame.Id)
            .WithName(_jame.UserName)
            .WithEmail(_jame.Email)
            .WithPhone(_jame.PhoneNumber)
            .WithRoles(_jame.Roles)
            .Build()
        };

        var resultDto = await identityAdapter.GetAllUserAsync();

        Assert.NotNull(resultDto);
        Assert.True(resultDto.Success);
        Assert.Collection(resultDto.Value!,
            penelope => {
                Assert.Equal(_penelope.Id, penelope.Id);
                Assert.Equal(_penelope.UserName, penelope.UserName);
                Assert.Equal(_penelope.Email, penelope.Email);
                Assert.Equal(_penelope.PhoneNumber, penelope.PhoneNumber);
                Assert.Equal(_penelope.Roles, penelope.Roles);
                },
            jame => {
                Assert.Equal(_jame.Id, jame.Id);
                Assert.Equal(_jame.UserName, jame.UserName);
                Assert.Equal(_jame.Email, jame.Email);
                Assert.Equal(_jame.PhoneNumber, jame.PhoneNumber);
                Assert.Equal(_jame.Roles, jame.Roles);
            });
    }

    [Fact]
    public async void ShouldNotGetAllUsers()
    {
        var error = InfrastructureError.Identity.ItDoesNotExistAnyUser;
        _identityService.Setup(x => x.GetAllUserAsync()).ReturnsAsync(_resultDtoCreator.CreateFailureResult<string?>(error));
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object, _resultDtoCreator);

        var resultDto = await identityAdapter.GetAllUserAsync();

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
        Assert.Equal(error.Title, resultDto.Error.Title);
        Assert.Equal(error.Code, resultDto.Error.Code);
        Assert.Equal(error.Detail, resultDto.Error.Detail);
        Assert.Equal(error.HttpStatusCode, resultDto.Error.HttpStatusCode);
    }

    [Fact]
    public async void ShouldGetUserById()
    {
        var userJson = JsonSerializer.Serialize(_jame);
        _identityService.Setup(x => x.GetUserByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(_resultDtoCreator.CreateSuccessResult<string?>(userJson));
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object, _resultDtoCreator);

        var resultDto = await identityAdapter.GetUserByIdAsync(Guid.NewGuid().ToString());

        Assert.NotNull(resultDto);
        Assert.True(resultDto.Success);
        Assert.Equal(_jame.Id, resultDto.Value!.Id);
        Assert.Equal(_jame.UserName, resultDto.Value!.UserName);
        Assert.Equal(_jame.Email, resultDto.Value!.Email);
        Assert.Equal(_jame.PhoneNumber, resultDto.Value!.PhoneNumber);
        Assert.Equal(_jame.Roles, resultDto.Value!.Roles);
    }

    [Fact]
    public async void ShouldNotGetUserById()
    {
        var error = InfrastructureError.Identity.ItDoesNotExistAnyUser;
        _identityService.Setup(x => x.GetUserByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(_resultDtoCreator.CreateFailureResult<string?>(error));
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object, _resultDtoCreator);

        var resultDto = await identityAdapter.GetUserByIdAsync(Guid.NewGuid().ToString());

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
        Assert.Equal(error.Title, resultDto.Error.Title);
        Assert.Equal(error.Code, resultDto.Error.Code);
        Assert.Equal(error.Detail, resultDto.Error.Detail);
        Assert.Equal(error.HttpStatusCode, resultDto.Error.HttpStatusCode);
    }

    [Fact]
    public void ShouldSayUserIsAssigned()
    {
        _identityService.Setup(x => x.IsUserIdAssigned(
            It.IsAny<string>()
            )).Returns(true);
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object, _resultDtoCreator);

        var notExist = identityAdapter.IsUserIdAssigned(Guid.NewGuid().ToString());

        Assert.True(notExist);
    }

    [Fact]
    public void ShouldSayUserIsNotAssigned()
    {
        _identityService.Setup(x => x.IsUserIdAssigned(
            It.IsAny<string>()
            )).Returns(false);
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object, _resultDtoCreator);

        var notExist = identityAdapter.IsUserIdAssigned(Guid.NewGuid().ToString());

        Assert.False(notExist);
    }

    [Fact]
    public async void ShouldUpdateUser()
    {
        _identityService.Setup(x => x.UpdateUserAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()
            )).ReturnsAsync(_resultDtoCreator.CreateSuccessResult());
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object, _resultDtoCreator);

        var resultDto = await identityAdapter.UpdateUserAsync("Jame", "oT586n@S&#nJ", "jame@auth.com", "111-222-3333");

        Assert.NotNull(resultDto);
        Assert.True(resultDto.Success);
    }

    [Fact]
    public async void ShouldNotUpdateUser()
    {
        var error = InfrastructureError.Identity.UserWasNotUpdated;
        _identityService.Setup(x => x.UpdateUserAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()
            )).ReturnsAsync(_resultDtoCreator.CreateFailureResult(error));
        IIdentityAdapter identityAdapter = new IdentityAdapter(_identityService.Object, _logger.Object, _resultDtoCreator);

        var resultDto = await identityAdapter.UpdateUserAsync("Jame", "oT586n@S&#nJ", "jame@auth.com", "111-222-3333");

        Assert.NotNull(resultDto);
        Assert.False(resultDto.Success);
        Assert.Equal(error.Title, resultDto.Error.Title);
        Assert.Equal(error.Code, resultDto.Error.Code);
        Assert.Equal(error.Detail, resultDto.Error.Detail);
        Assert.Equal(error.HttpStatusCode, resultDto.Error.HttpStatusCode);
        _logger.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<ErrorDto>()), Times.Once);
    }
}
