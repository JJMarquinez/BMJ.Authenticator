using BMJ.Authenticator.Domain.Entities.Users;
using BMJ.Authenticator.Domain.ValueObjects;

namespace BMJ.Authenticator.Domain.UnitTests.Entities.Users;

public class UserTests
{
    User _user;
    string _userId;
    string _userName;
    string _email;
    string[] _roles;
    string _phone;
    string _password;


    public UserTests()
    {
        _userId = Guid.NewGuid().ToString();
        _userName = "Jaden";
        _email = "jaden@authenticator.com";
        _roles = new[] { "Standard" };
        _phone = "111-222-3333";
        _password = Guid.NewGuid().ToString();
        _user = User.New(
            _userId, 
            _userName, 
            Email.From(_email), 
            _roles, 
            Phone.New(_phone), 
            _password);
    }

    [Fact]
    public void Should_ThrowArgumentNullException_When_IsAttemptToBeCreatedWithNullId()
    {
        Assert.Throws<ArgumentNullException>(() 
            => { 
                User.New(null, _userName, Email.From(_email), null, null, null); 
            });
    }

    [Fact]
    public void Should_ThrowArgumentException_When_IsAttemptToBeCreatedWithEmptyId()
    {
        Assert.Throws<ArgumentException>(()
            => {
                User.New(string.Empty, _userName, Email.From(_email), null, null, null);
            });
    }

    [Fact]
    public void Should_ThrowArgumentNullException_When_IsAttemptToBeCreatedWithNullUserName()
    {
        Assert.Throws<ArgumentNullException>(()
            => {
                User.New(_userId, null, Email.From(_email), null, null, null);
            });
    }

    [Fact]
    public void Should_ThrowArgumentException_When_IsAttemptToBeCreatedWithEmptyUserName()
    {
        Assert.Throws<ArgumentException>(()
            => {
                User.New(_userId, string.Empty, Email.From(_email), null, null, null);
            });
    }

    [Fact]
    public void Should_ThrowArgumentNullException_When_IsAttemptToBeCreatedWithNullEmail()
    {
        Assert.Throws<ArgumentNullException>(()
            => {
                User.New(_userId, _userName, null, null, null, null);
            });
    }

    [Fact]
    public void Should_CreateNewUser_When_ParametersAreValid()
    {
        Assert.NotNull(User.New(_userId, _userName, Email.From(_email), null, null, null));
    }

    [Fact]
    public void Should_GetId_When_UserIsCreated()
    {
        Assert.Equal(_userId, _user.GetId());
    }

    [Fact]
    public void Should_GetUserName_When_UserIsCreated()
    {
        Assert.Equal(_userName, _user.GetUserName());
    }

    [Fact]
    public void Should_GetEmail_When_UserIsCreated()
    {
        Assert.Equal(_email, _user.GetEmail());
    }

    [Fact]
    public void Should_GetRoles_When_UserIsCreated()
    {
        Assert.Equal(_roles, _user.GetRoles());
    }

    [Fact]
    public void Should_GetPhone_When_UserIsCreated()
    {
        Assert.Equal(_phone, _user.GetPhoneNumber());
    }

    [Fact]
    public void Should_GetPassword_When_UserIsCreated()
    {
        Assert.Equal(_password, _user.GetPasswordHash());
    }
}
