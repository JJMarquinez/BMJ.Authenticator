using BMJ.Authenticator.Adapter.Common;
using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Application.FunctionalTests.TestContext;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers;

namespace BMJ.Authenticator.Application.FunctionalTests.UseCases.Users.Queries.GetAllUsers;

[Collection(nameof(AuthenticatorTestConextCollection))]
public class GetAllUsersQueryHandlerTests : IAsyncLifetime
{
    private readonly AuthenticatorTestConext _testContext;
    private readonly Func<Task> _resetDatabase;

    public GetAllUsersQueryHandlerTests(AuthenticatorTestConext testConext)
    {
        _testContext = testConext;
        _resetDatabase = _testContext.ResetState;
    }

    public Task DisposeAsync() => _resetDatabase();

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async ValueTask ShouldReturnAllUsers()
    {
        var user = new UserDto
        { 
            UserName = "Joe",
            Email = "joe@authenticator.com",
            PhoneNumber = "111-444-777",
            Roles = new[] { "Guest" }
        };
        await _testContext.AddAsync(user, "M6#?m412kNSH");

        var query = new GetAllUsersQuery();
        var result = await _testContext.SendAsync(query);
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.True(result.Value!.Count == 1);
        Assert.Collection(result.Value!,
            userDto =>
            {
                Assert.Equal("Joe", userDto.UserName);
                Assert.Equal("joe@authenticator.com", userDto.Email);
                Assert.Equal("111-444-777", userDto.PhoneNumber);
                Assert.Equal("Guest", userDto.Roles[0]);
            });
    }

    [Fact]
    public async ValueTask ShouldNotReturnAnyUsers()
    {
        var error = InfrastructureError.Identity.ItDoesNotExistAnyUser;
        var query = new GetAllUsersQuery();
        var result = await _testContext.SendAsync(query);

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(error.Title, result.Error.Title);
        Assert.Equal(error.Detail, result.Error.Detail);
        Assert.Equal(error.Code, result.Error.Code);
        Assert.Equal(error.HttpStatusCode, result.Error.HttpStatusCode);
    }
}
