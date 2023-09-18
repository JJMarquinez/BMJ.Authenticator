using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Results.FactoryMethods;
using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers;
using MediatR;
using Moq;

namespace BMJ.Authenticator.Application.UnitTests.UseCases.Queries.GetAllUsers;

public class GetAllUsersQueryHandlerTests
{
    private readonly Mock<IIdentityAdapter> _identityAdapter;
    private readonly UserDto _jame, _penelope;
    private readonly IResultDtoCreator _resultDtoCreator;

    public GetAllUsersQueryHandlerTests()
    {
        _identityAdapter = new();
        _resultDtoCreator = new ResultDtoCreator(new ResultDtoFactory(), new ResultDtoGenericFactory());

        _jame = new UserDto
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Jame",
            Email = "jame@auth.com",
            PhoneNumber = "111-222-3333",
            Roles = new[] { "Standard" }
        };
        _penelope = new UserDto
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Penelope",
            Email = "penelope@auth.com",
            PhoneNumber = "444-222-3333",
            Roles = new[] { "Administrator" }
        };
    }

    [Fact]
    public async void ShouldGetAllUsers()
    {
        var query = new GetAllUsersQuery();
        var token = new CancellationTokenSource().Token;
        var userDtoList = new List<UserDto>
        {
            new UserDto() { Id = _penelope.Id, UserName = _penelope.UserName, Email = _penelope.Email, PhoneNumber = _penelope.PhoneNumber, Roles = _penelope.Roles },
            new UserDto() { Id = _jame.Id, UserName = _jame.UserName, Email = _jame.Email, PhoneNumber = _jame.PhoneNumber, Roles = _jame.Roles },
        };
        _identityAdapter.Setup(x => x.GetAllUserAsync()).ReturnsAsync(_resultDtoCreator.CreateSuccessResult<List<UserDto>?>(userDtoList));
        IRequestHandler<GetAllUsersQuery, ResultDto<List<UserDto>?>> handler = new GetAllUsersQueryHandler(_identityAdapter.Object);
        var resultDto = await handler.Handle(query, token);

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
}
