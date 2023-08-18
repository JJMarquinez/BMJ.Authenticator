using BMJ.Authenticator.Domain.Entities.Users;
using BMJ.Authenticator.Domain.ValueObjects;

namespace BMJ.Authenticator.Domain.UnitTests.Entities.Users;

public class UserTests
{
    string _userId;
    string _userName;
    string _email;

    public UserTests()
    {
        _userId = Guid.NewGuid().ToString();
        _userName = "Jaden";
        _email = "jaden@authenticator.com";
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullId()
    {
        Assert.Throws<ArgumentNullException>(() 
            => { 
                User.Builder()
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
                User.Builder()
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
                User.Builder()
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
                User.Builder()
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
                User.Builder()
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
            User.Builder()
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
        User user = User.Builder()
                .WithId(id)
                .WithName(_userName)
                .WithEmail(Email.From(_email))
                .Build();

        Assert.Equal(id, user.GetId());
    }

    [Theory]
    [InlineData("Angel")]
    [InlineData("MEelna")]
    [InlineData("crsanchez")]
    public void ShouldGetUserNameGivenACreatedUser(string userName)
    {
        User user =
            User.Builder()
                .WithId(_userId)
                .WithName(userName)
                .WithEmail(Email.From(_email))
                .Build();

        Assert.Equal(userName, user.GetUserName());
    }

    [Theory]
    [InlineData("andres@jmb.com")]
    [InlineData("jaime@localhost.es")]
    [InlineData("sebas.gomez@test.cat")]
    public void ShouldGetEmailGivenACreatedUser(string address)
    {
        User user = User.Builder()
                .WithId(_userId)
                .WithName(_userName)
                .WithEmail(Email.From(address))
                .Build();

        Assert.Equal(address, user.GetEmail());
    }

    [Theory]
    [InlineData("Administrator", "Standard")]
    [InlineData("SalesMan", "Manager")]
    [InlineData("CEO", "Developer")]
    public void ShouldGetRolesGivenACreatedUser(string firstRole, string secondRole)
    {
        string[] roles = new[] { firstRole, secondRole };
        User user = User.Builder()
                .WithId(_userId)
                .WithName(_userName)
                .WithEmail(Email.From(_email))
                .WithRoles(roles)
                .Build();

        Assert.Equal(roles, user.GetRoles());
    }

    [Theory]
    [InlineData("673 921 4850")]
    [InlineData("673.921.4850")]
    [InlineData("673-921-4850")]
    public void ShouldGetPhoneGivenACreatedUser(string phoneNumber)
    {
        User user = User.Builder()
                .WithId(_userId)
                .WithName(_userName)
                .WithEmail(Email.From(_email))
                .WithPhone(Phone.New(phoneNumber))
                .Build();

        Assert.Equal(phoneNumber, user.GetPhoneNumber()!);
    }

    [Theory]
    [InlineData("Ts!39H1z")]
    [InlineData("6V5$zRg2")]
    [InlineData("#553zP1k")]
    public void ShouldGetPasswordGivenACreatedUser(string password)
    {
        User user = User.Builder()
                .WithId(_userId)
                .WithName(_userName)
                .WithEmail(Email.From(_email))
                .WithPasswordHash(password)
                .Build();

        Assert.Equal(password, user.GetPasswordHash());
    }
}
