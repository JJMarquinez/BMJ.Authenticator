using BMJ.Authenticator.Domain.Entities.Users;
using BMJ.Authenticator.Domain.Entities.Users.Builders;
using BMJ.Authenticator.Domain.ValueObjects;

namespace BMJ.Authenticator.Domain.UnitTests.Entities.Users;

public class UserTests
{
    private readonly string _userId;
    private readonly string _userName;
    private readonly string _email;
    private readonly IUserBuilder _userBuilder;

    public UserTests()
    {
        _userId = Guid.NewGuid().ToString();
        _userName = "Jaden";
        _email = "jaden@authenticator.com";
        _userBuilder = new UserBuilder();
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullId()
    {
        Assert.Throws<ArgumentNullException>(() 
            => { 
                _userBuilder
                .WithId(null!)
                .WithName(_userName)
                .WithEmail(Email.From(_email))
                .Build();
            });
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenEmptyId()
    {
        Assert.Throws<ArgumentException>(()
            => {
                _userBuilder
                .WithId(string.Empty)
                .WithName(_userName)
                .WithEmail(Email.From(_email))
                .Build();
            });
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullUserName()
    {
        Assert.Throws<ArgumentNullException>(()
            => {
                _userBuilder
                .WithId(_userId)
                .WithName(null!)
                .WithEmail(Email.From(_email))
                .Build();
            });
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenEmptyUserName()
    {
        Assert.Throws<ArgumentException>(()
            => {
                _userBuilder
                .WithId(_userId)
                .WithName(string.Empty)
                .WithEmail(Email.From(_email))
                .Build();
            });
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullEmail()
    {
        Assert.Throws<ArgumentNullException>(()
            => {
                _userBuilder
                .WithId(_userId)
                .WithName(_userName)
                .WithEmail(null!)
                .Build();
            });
    }

    [Fact]
    public void ShouldCreateNewUser()
    {
        Assert.NotNull(
            _userBuilder
                .WithId(_userId)
                .WithName(_userName)
                .WithEmail(Email.From(_email))
                .Build());
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("asdfg")]
    [InlineData("qw98e")]
    public void ShouldGetIdGivenACreatedUser(string id)
    {
        User user = _userBuilder
                .WithId(id)
                .WithName(_userName)
                .WithEmail(Email.From(_email))
                .Build();

        Assert.Equal(id, user.Id);
    }

    [Theory]
    [InlineData("Angel")]
    [InlineData("MEelna")]
    [InlineData("crsanchez")]
    public void ShouldGetUserNameGivenACreatedUser(string userName)
    {
        User user =
            _userBuilder
                .WithId(_userId)
                .WithName(userName)
                .WithEmail(Email.From(_email))
                .Build();

        Assert.Equal(userName, user.UserName);
    }

    [Theory]
    [InlineData("andres@jmb.com")]
    [InlineData("jaime@localhost.es")]
    [InlineData("sebas.gomez@test.cat")]
    public void ShouldGetEmailGivenACreatedUser(string address)
    {
        User user = _userBuilder
                .WithId(_userId)
                .WithName(_userName)
                .WithEmail(Email.From(address))
                .Build();

        Assert.Equal(address, user.Email);
    }

    [Theory]
    [InlineData("Administrator", "Standard")]
    [InlineData("SalesMan", "Manager")]
    [InlineData("CEO", "Developer")]
    public void ShouldGetRolesGivenACreatedUser(string firstRole, string secondRole)
    {
        string[] roles = new[] { firstRole, secondRole };
        User user = _userBuilder
                .WithId(_userId)
                .WithName(_userName)
                .WithEmail(Email.From(_email))
                .WithRoles(roles)
                .Build();

        Assert.Equal(roles, user.Roles);
    }

    [Theory]
    [InlineData("673 921 4850")]
    [InlineData("673.921.4850")]
    [InlineData("673-921-4850")]
    public void ShouldGetPhoneGivenACreatedUser(string phoneNumber)
    {
        User user = _userBuilder
                .WithId(_userId)
                .WithName(_userName)
                .WithEmail(Email.From(_email))
                .WithPhone(Phone.New(phoneNumber))
                .Build();

        Assert.Equal(phoneNumber, user.PhoneNumber!);
    }
}
