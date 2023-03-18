using BMJ.Authenticator.Domain.ValueObjects;
using System.Net;

namespace BMJ.Authenticator.Domain.UnitTests.ValueObjects;

public class EmailTest
{
    [Theory]
    [InlineData("andres@jmb.com")]
    [InlineData("jaime@localhost.es")]
    [InlineData("sebas.gomez@test.cat")]
    public void Should_BeValidEmail_When_AddressFullfilsTheEmailPattern(string address)
    {
        Email email = Email.From(address);
        Assert.Equal(address, email.ToString());
    }

    [Theory]
    [InlineData("andres@jmb.com")]
    [InlineData("jaime@localhost.es")]
    [InlineData("sebas.gomez@test.cat")]
    public void Should_ConvertToString_When_PerformImplicitOperator(string address)
    {
        string email = Email.From(address);
        Assert.Equal(email, address);
    }

    [Theory]
    [InlineData("andres@jmb.com")]
    [InlineData("jaime@localhost.es")]
    [InlineData("sebas.gomez@test.cat")]
    public void Should_ConvertToEmail_When_PerformExplicitOperator(string address)
    {
        Email email = (Email)address;
        Assert.True(email.Equals(Email.From(address)));
    }

    [Theory]
    [InlineData("andres.jmb.com")]
    [InlineData("@localhost.es")]
    [InlineData("")]
    public void Should_ThrowArgumentException_When_AddressDoesNotFullfitEmailPattern(string address)
    {
        Assert.Throws<ArgumentException>(() => Email.From(address));
    }

    [Fact]
    public void Should_ThrowArgumentNullException_When_AddressIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Email.From(null));
    }

    [Fact]
    public void Should_EmailBeDifference_When_CompareToOtherObject()
    {
        Assert.False(Email.From("sebas.gomez@test.cat").Equals(new object()));
    }

    [Fact]
    public void Should_EmailBeDifference_When_CompareEmailToNull()
    {
        Assert.False(Email.From("sebas.gomez@test.cat").Equals(null));
    }

    [Fact]
    public void Should_BeInteger_When_GetHashCode()
    {
        Assert.IsType<int>(Email.From("sebas.gomez@test.cat").GetHashCode());
    }
}