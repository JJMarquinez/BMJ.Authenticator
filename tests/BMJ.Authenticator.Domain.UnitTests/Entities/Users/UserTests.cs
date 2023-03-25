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
                User.New(null!, _userName, Email.From(_email), null, null, null!); 
            });
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenEmptyId()
    {
        Assert.Throws<ArgumentException>(()
            => {
                User.New(string.Empty, _userName, Email.From(_email), null, null, null!);
            });
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullUserName()
    {
        Assert.Throws<ArgumentNullException>(()
            => {
                User.New(_userId, null!, Email.From(_email), null, null, null!);
            });
    }

    [Fact]
    public void ShouldThrowArgumentExceptionGivenEmptyUserName()
    {
        Assert.Throws<ArgumentException>(()
            => {
                User.New(_userId, string.Empty, Email.From(_email), null, null, null!);
            });
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullEmail()
    {
        Assert.Throws<ArgumentNullException>(()
            => {
                User.New(_userId, _userName, null!, null, null, null!);
            });
    }

    [Fact]
    public void ShouldCreateNewUser()
    {
        Assert.NotNull(User.New(_userId, _userName, Email.From(_email), null, null, null!));
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("asdfg")]
    [InlineData("qw98e")]
    public void ShouldGetIdGivenACreatedUser(string id)
    {
        User user = User.New(id, _userName, Email.From(_email), null, null, null!);
        Assert.Equal(id, user.GetId());
    }

    [Theory]
    [InlineData("Angel")]
    [InlineData("MEelna")]
    [InlineData("crsanchez")]
    public void ShouldGetUserNameGivenACreatedUser(string userName)
    {
        User user = User.New(_userId, userName, Email.From(_email), null, null, null!);
        Assert.Equal(userName, user.GetUserName());
    }

    [Theory]
    [InlineData("andres@jmb.com")]
    [InlineData("jaime@localhost.es")]
    [InlineData("sebas.gomez@test.cat")]
    public void ShouldGetEmailGivenACreatedUser(string address)
    {
        User user = User.New(_userId, _userName, Email.From(address), null, null, null!);
        Assert.Equal(address, user.GetEmail());
    }

    [Theory]
    [InlineData("Administrator", "Standard")]
    [InlineData("SalesMan", "Manager")]
    [InlineData("CEO", "Developer")]
    public void ShouldGetRolesGivenACreatedUser(string firstRole, string secondRole)
    {
        string[] roles = new[] { firstRole, secondRole };
        User user = User.New(_userId, _userName, Email.From(_email), roles, null, null!);
        Assert.Equal(roles, user.GetRoles());
    }

    [Theory]
    [InlineData("673 921 4850")]
    [InlineData("673.921.4850")]
    [InlineData("673-921-4850")]
    public void ShouldGetPhoneGivenACreatedUser(string phoneNumber)
    {
        User user = User.New(_userId, _userName, Email.From(_email), null, Phone.New(phoneNumber), null!);
        Assert.Equal(phoneNumber, user.GetPhoneNumber()!);
    }

    [Theory]
    [InlineData("Ts!39H1z")]
    [InlineData("6V5$zRg2")]
    [InlineData("#553zP1k")]
    public void ShouldGetPasswordGivenACreatedUser(string password)
    {
        User user = User.New(_userId, _userName, Email.From(_email), null, null, password);
        Assert.Equal(password, user.GetPasswordHash());
    }
}
