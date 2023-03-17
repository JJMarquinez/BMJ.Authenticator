using BMJ.Authenticator.Domain.ValueObjects;

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
        Assert.Equal(email, Email.From(address));
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
}