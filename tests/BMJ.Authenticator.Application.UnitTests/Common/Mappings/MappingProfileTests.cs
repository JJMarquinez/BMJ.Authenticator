using AutoMapper;
using BMJ.Authenticator.Application.Common.Mappings;
using BMJ.Authenticator.Application.Common.Models;

namespace BMJ.Authenticator.Application.UnitTests.Common.Mappings;

public class MappingProfileTests
{
    [Fact]
    public void ShouldMapErrorToErrorDto()
    {
        var profile = new MappingProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
        var execption = Record.Exception(configuration.AssertConfigurationIsValid);
        Assert.Null(execption);
    }
}
