using AutoMapper;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Domain.Common.Errors.Builders;
using BMJ.Authenticator.Domain.Common.Results.Factories;

namespace BMJ.Authenticator.Application.UnitTests.Common.Models.Results;

public class ResultTDtoTests
{
    private readonly ResultMappingProfile _profile;
    private readonly IResultCreator _resultCreator;

    public ResultTDtoTests()
    {
        _profile = new ResultMappingProfile();
        _resultCreator = new ResultCreator(new ResultFactory(), new ResultGenericFactory());
    }

    [Fact]
    public void ShouldMapSuccessResultToResultDto()
    {
        new ResultDto<object>().Mapping(_profile);
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(_profile));
        configuration.AssertConfigurationIsValid();

        var result = configuration.CreateMapper().Map<ResultDto<object>>(_resultCreator.CreateSuccessResult(new object()));

        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public void ShouldMapFailureResultToResultDto()
    {
        new ResultDto<object>().Mapping(_profile);
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(_profile));
        configuration.AssertConfigurationIsValid();
        var error = new ErrorBuilder()
            .WithCode("Identity.Argument.UserNameOrPasswordNotValid")
            .WithTitle("User name or password aren't valid.")
            .WithDetail("The user name or password wich were sent are not correct, either the user doesn't exist or password isn't correct.")
            .WithHttpStatusCode(409)
            .Build();

        var result = configuration.CreateMapper().Map<ResultDto<object>>(_resultCreator.CreateFailureResult<object>(error));

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(error.Title, result.Error.Title);
        Assert.Equal(error.Code, result.Error.Code);
        Assert.Equal(error.Detail, result.Error.Detail);
        Assert.Equal(error.HttpStatusCode, result.Error.HttpStatusCode);
    }
}
