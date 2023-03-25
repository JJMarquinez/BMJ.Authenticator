using BMJ.Authenticator.Domain.ValueObjects;
using System.Net;

namespace BMJ.Authenticator.Domain.UnitTests.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("andres@jmb.com")]
    [InlineData("jaime@localhost.es")]
    [InlineData("sebas.gomez@test.cat")]
    public void ShouldCreateAnEmailGivenValidAddress(string address)
    {
        Email email = Email.From(address);
        Assert.Equal(address, email.ToString());
    }

    [Theory]
    [InlineData("andres@jmb.com")]
    [InlineData("jaime@localhost.es")]
    [InlineData("sebas.gomez@test.cat")]
    public void ShouldConvertToString(string address)
    {
        string email = Email.From(address);
        Assert.Equal(email, address);
    }

    [Theory]
    [InlineData("andres@jmb.com")]
    [InlineData("jaime@localhost.es")]
    [InlineData("sebas.gomez@test.cat")]
    public void ShouldConvertToEmail(string address)
    {
        Email email = (Email)address;
        Assert.True(email.Equals(Email.From(address)));
    }

    [Theory]
    [InlineData("andres.jmb.com")]
    [InlineData("@localhost.es")]
    [InlineData("")]
    public void ShouldThrowArgumentExceptionGivenInvalidAddress(string address)
    {
        Assert.Throws<ArgumentException>(() => Email.From(address));
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullAddress()
    {
        Assert.Throws<ArgumentNullException>(() => Email.From(null!));
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenEmptyStringAsAddress()
    {
        Assert.Throws<ArgumentException>(() => Email.From(string.Empty));
    }

    [Fact]
    public void ShouldEmailBeDifferenceGivenDifferentObject()
    {
        Assert.False(Email.From("sebas.gomez@test.cat").Equals(new object()));
    }

    [Fact]
    public void ShouldEmailBeDifferenceGivenNullToCompareWith()
    {
        Assert.False(Email.From("sebas.gomez@test.cat").Equals(null));
    }

    [Fact]
    public void ShouldCreateEmail()
    {
        Assert.NotNull(Email.From("sebas.gomez@test.cat"));
    }
}