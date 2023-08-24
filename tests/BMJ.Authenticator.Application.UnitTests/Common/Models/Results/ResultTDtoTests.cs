using AutoMapper;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Domain.Common.Results;

namespace BMJ.Authenticator.Application.UnitTests.Common.Models.Results;

public class ResultTDtoTests
{
    private readonly ResultMappingProfile _profile;
    public ResultTDtoTests()
    {
        _profile = new ResultMappingProfile();
    }

    [Fact]
    public void ShouldMapSuccessResultToResultDto()
    {
        new ResultDto<object>().Mapping(_profile);
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(_profile));
        configuration.AssertConfigurationIsValid();
        var result = configuration.CreateMapper().Map<ResultDto<object>>(Result.Success(new object()));

        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public void ShouldMapFailureResultToResultDto()
    {
        new ResultDto<object>().Mapping(_profile);
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(_profile));
        configuration.AssertConfigurationIsValid();
        var error = Domain.Common.Errors.Error.None;
        var result = configuration.CreateMapper().Map<ResultDto<object>>(Result.Failure<object>(error));

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(error.Title, result.Error.Title);
        Assert.Equal(error.Code, result.Error.Code);
        Assert.Equal(error.Detail, result.Error.Detail);
        Assert.Equal(error.HttpStatusCode, result.Error.HttpStatusCode);
    }
}
