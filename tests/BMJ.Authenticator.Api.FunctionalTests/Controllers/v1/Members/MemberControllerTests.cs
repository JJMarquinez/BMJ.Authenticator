using BMJ.Authenticator.Adapter.Common;
using BMJ.Authenticator.Api.FunctionalTests.TestContext;
using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById;
using BMJ.Authenticator.Application.UseCases.Users.Queries.LoginUser;
using BMJ.Authenticator.Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace BMJ.Authenticator.Api.FunctionalTests.Controllers.v1.Members;

[Collection(nameof(AuthenticatorTestConextCollection))]
public class MemberControllerTests : IAsyncLifetime
{
    private readonly AuthenticatorTestConext _testContext;
    private readonly Func<Task> _resetDatabase;

    public MemberControllerTests(AuthenticatorTestConext testContext)
    {
        _testContext = testContext;
        _resetDatabase = _testContext.ResetState;
    }

    public Task DisposeAsync() => _resetDatabase();

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ShouldGetToken()
    {
        var applicationUser = ApplicationUser.Builder()
            .WithUserName("Megan")
            .WithEmail("megan@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        await _testContext.AddAsync(applicationUser, "A@9&53ro1XG-", roles);

        var request = new LoginUserQuery
        {
            UserName = "Megan",
            Password = "A@9&53ro1XG-"
        };

        var response = await _testContext.GetAsync(AuthenticatorApi.GetTokenAsync(), request);
        var token = await response.Content.ReadAsStringAsync();

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrEmpty(token));
    }

    [Fact]
    public async Task ShouldNotGetTokenGivenInvalidCredentials()
    {
        var applicationUser = ApplicationUser.Builder()
            .WithUserName("Megan")
            .WithEmail("megan@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        await _testContext.AddAsync(applicationUser, "A@9&53ro1XG-", roles);

        var request = new LoginUserQuery
        {
            UserName = "Megan",
            Password = "A@9&53ro1X"
        };

        var response = await _testContext.GetAsync(AuthenticatorApi.GetTokenAsync(), request);
        var result = await response.Content.ReadAsStringAsync();
        var problemDetail = JsonSerializer.Deserialize<ProblemDetails>(result);
        problemDetail!.Extensions.TryGetValue("errorCode", out var errorCode);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.NotNull(problemDetail);
        Assert.Equal(AuthenticatorApi.GetTokenAsync(), problemDetail.Instance);
    }

    [Fact]
    public async Task ShouldNotGetTokenMisingPassword()
    {
        var request = new LoginUserQuery { UserName = "Megan" };

        var response = await _testContext.GetAsync(AuthenticatorApi.GetTokenAsync(), request);
        var result = await response.Content.ReadAsStringAsync();
        var problemDetail = JsonSerializer.Deserialize<ProblemDetails>(result);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(problemDetail);
        Assert.Equal(400, problemDetail.Status);
        Assert.Equal("One or more validation errors occurred.", problemDetail.Title);
    }

    [Fact]
    public async Task ShouldNotGetTokenMisingUsername()
    {
        var request = new LoginUserQuery { Password = "A@9&53ro1X" };

        var response = await _testContext.GetAsync(AuthenticatorApi.GetTokenAsync(), request);
        var result = await response.Content.ReadAsStringAsync();
        var problemDetail = JsonSerializer.Deserialize<ProblemDetails>(result);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(problemDetail);
        Assert.Equal(400, problemDetail.Status);
        Assert.Equal("One or more validation errors occurred.", problemDetail.Title);
    }

    [Fact]
    public async Task ShouldNotGetTokenMisingUsernameAndPassword()
    {
        var request = new LoginUserQuery();

        var response = await _testContext.GetAsync(AuthenticatorApi.GetTokenAsync(), request);
        var result = await response.Content.ReadAsStringAsync();
        var problemDetail = JsonSerializer.Deserialize<ProblemDetails>(result);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(problemDetail);
        Assert.Equal(400, problemDetail.Status);
        Assert.Equal("One or more validation errors occurred.", problemDetail.Title);
    }

    [Fact]
    public async Task ShouldNotGetAnyUsersGivenAnonymousRequest()
    {
        var request = new GetAllUsersQuery();

        var response = await _testContext.GetAsync(AuthenticatorApi.GetAllAsync(), request);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ShouldGetAllUsers()
    {
        var token = await _testContext.GetTokenAsync();
        var applicationUser = ApplicationUser.Builder()
            .WithUserName("Megan")
            .WithEmail("megan@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);
        var request = new GetAllUsersQuery();

        var response = await _testContext.GetAsync(AuthenticatorApi.GetAllAsync(), request, token);
        var result = await response.Content.ReadAsStringAsync();
        var userDtoList = JsonSerializer.Deserialize<List<UserDto>?>(result);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(userDtoList);
        Assert.True(userDtoList.Any());
    }

    [Fact]
    public async Task ShouldNotGetUserByIdGivenAnonymousRequest()
    {
        var request = new GetUserByIdQuery { Id = Guid.NewGuid().ToString() };

        var response = await _testContext.GetAsync(AuthenticatorApi.GetByIdAsync(), request);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ShouldGetUserById()
    {
        var applicationUser = ApplicationUser.Builder()
            .WithUserName("Megan")
            .WithEmail("megan@authenticator.com")
            .WithPhoneNumber("111-444-777")
            .Build();
        var roles = new[] { "Guest" };
        string? userId = await _testContext.AddAsync(applicationUser, "M6#?m412kNSH", roles);

        var token = await _testContext.GetTokenAsync();
        var request = new GetUserByIdQuery { Id = userId };

        var response = await _testContext.GetAsync(AuthenticatorApi.GetByIdAsync(), request, token);
        var result = await response.Content.ReadAsStringAsync();
        var userDtoResult = JsonSerializer.Deserialize<UserDto?>(result);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(userDtoResult);
    }

    [Fact]
    public async Task ShouldNotGetUserByIdGivenEmptyUserId()
    {
        var request = new GetUserByIdQuery();
        var token = await _testContext.GetTokenAsync();

        var response = await _testContext.GetAsync(AuthenticatorApi.GetByIdAsync(), request, token);
        var result = await response.Content.ReadAsStringAsync();
        var problemDetail = JsonSerializer.Deserialize<ProblemDetails>(result);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(problemDetail);
        Assert.Equal(400, problemDetail.Status);
        Assert.Equal("One or more validation errors occurred.", problemDetail.Title);
    }

    [Fact]
    public async Task ShouldNotGetUserByIdGivenNonExistingUserId()
    {
        var request = new GetUserByIdQuery { Id = Guid.NewGuid().ToString() };
        var token = await _testContext.GetTokenAsync();

        var response = await _testContext.GetAsync(AuthenticatorApi.GetByIdAsync(), request, token);
        var result = await response.Content.ReadAsStringAsync();
        var problemDetail = JsonSerializer.Deserialize<ProblemDetails>(result);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(problemDetail);
        Assert.Equal(400, problemDetail.Status);
        Assert.Equal("One or more validation errors occurred.", problemDetail.Title);
    }
}
