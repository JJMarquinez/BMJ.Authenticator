using BMJ.Authenticator.Adapter.Common;
using BMJ.Authenticator.Api.FunctionalTests.TestContext;
using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Application.UseCases.Users.Queries.LoginUser;
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
        var userDto = new UserDto
        {
            UserName = "Megan",
            Email = "megan@authenticator.com",
            PhoneNumber = "111-444-777",
            Roles = new[] { "Guest" }
        };
        await _testContext.AddAsync(userDto, "A@9&53ro1XG-");
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
        var error = InfrastructureError.Identity.UserNameOrPasswordNotValid;
        var userDto = new UserDto
        {
            UserName = "Megan",
            Email = "megan@authenticator.com",
            PhoneNumber = "111-444-777",
            Roles = new[] { "Guest" }
        };
        await _testContext.AddAsync(userDto, "A@9&53ro1XG-");
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
        Assert.Equal(error.Title, problemDetail.Title);
        Assert.Equal(error.HttpStatusCode, problemDetail.Status);
        Assert.Equal(error.Detail, problemDetail.Detail);
        Assert.Equal(AuthenticatorApi.GetTokenAsync(), problemDetail.Instance);
        Assert.Equal(error.Code, errorCode!.ToString());
    }

    [Fact]
    public async Task ShouldNotGetTokenMisingPassword()
    {
        var request = new LoginUserQuery
        {
            UserName = "Megan"
        };

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
        var request = new LoginUserQuery
        {
            Password = "A@9&53ro1X"
        };

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
}
