using AutoMapper;
using BMJ.Authenticator.Application.Common.Mappings;
using BMJ.Authenticator.Application.Common.Models;
using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Application.Common.Models.Users.Builders;
using BMJ.Authenticator.Domain.Common.Errors;
using BMJ.Authenticator.Domain.Entities.Users;

namespace BMJ.Authenticator.Application.UnitTests.Common.Mappings;

public class IMapFromTests
{
    private readonly MapFromProfile _profile;
    public IMapFromTests()
    {
        _profile = new MapFromProfile();
    }

    [Fact]
    public void ShouldMapErrorToErrorDto()
    {
        IMapFrom<Error> mapFrom = new ErrorDto();
        mapFrom.Mapping(_profile);
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(_profile));
        configuration.AssertConfigurationIsValid();
        var errorDto = configuration.CreateMapper().Map<ErrorDto>(Error.None);

        Assert.NotNull(errorDto);
        Assert.Equal(Error.None.Title, errorDto.Title);
        Assert.Equal(Error.None.Code, errorDto.Code);
        Assert.Equal(Error.None.Detail, errorDto.Detail);
        Assert.Equal(Error.None.HttpStatusCode, errorDto.HttpStatusCode);
    }

    [Fact]
    public void ShouldNotMapErrorToErrorDto()
    {
        IMapFrom<User> mapFrom = new UserDtoBuilder().Build();
        mapFrom.Mapping(_profile);
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(_profile));
        configuration.AssertConfigurationIsValid();
        Assert.Throws<AutoMapperMappingException>(() => configuration.CreateMapper().Map<ErrorDto>(Error.None));
    }
}
