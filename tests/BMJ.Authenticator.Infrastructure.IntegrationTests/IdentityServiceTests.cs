using BMJ.Authenticator.Adapter.Common;
using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Infrastructure.Identity;
using BMJ.Authenticator.Infrastructure.IntegrationTests.TextContext;
using System.Text.Json;

namespace BMJ.Authenticator.Infrastructure.IntegrationTests;

[Collection(nameof(AuthenticatorTestConextCollection))]
public class IdentityServiceTests : IAsyncLifetime
{
    private readonly AuthenticatorTestConext _testContext;
    private readonly Func<Task> _resetDatabase;

    public IdentityServiceTests(AuthenticatorTestConext testContext)
    {
        _testContext = testContext;
        _resetDatabase = _testContext.ResetState;
    }

    public Task DisposeAsync() => _resetDatabase();

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ShouldNotGetAnyUsersGivenNoUserSaved()
    {
        var identityService = _testContext.GetIdentityService();

        var result = await identityService.GetAllUserAsync();

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(InfrastructureError.Identity.ItDoesNotExistAnyUser, result.Error);
    }

    [Fact]
    public async Task ShouldGetAllUsers()
    {
        var userDto = new UserDto
        {
            UserName = "Joe",
            Email = "joe@authenticator.com",
            PhoneNumber = "111-444-777",
            Roles = new[] { "Guest" }
        };
        var userId = await _testContext.AddAsync(userDto, "M6#?m412kNSH");
        var identityService = _testContext.GetIdentityService();

        var result = await identityService.GetAllUserAsync();
        var resultValue = JsonSerializer.Deserialize<List<UserIdentification>?>(result.Value!);

        Assert.True(result.Success);
        Assert.Single(resultValue!);
         Assert.Collection(resultValue!,
            user =>
            {
                Assert.Equal(userId, user.Id);
                Assert.Equal(userDto.UserName, user.UserName);
                Assert.Equal(userDto.Email, user.Email);
                Assert.Equal(userDto.PhoneNumber, user.PhoneNumber!);
                Assert.Equal(userDto.Roles, user.Roles!);
            });
    }
}
