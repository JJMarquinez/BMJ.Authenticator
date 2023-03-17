using BMJ.Authenticator.Domain.ValueObjects;

namespace BMJ.Authenticator.Domain.UnitTests.ValueObjects;

public class PhoneTest
{
    [Theory]
    [InlineData("673 921 4850")]
    [InlineData("673.921.4850")]
    [InlineData("673-921-4850")]
    public void Should_BeValidPhone_When_NumberFullfilsThePhonePattern(string number)
    {
        Phone phone = Phone.New(number);
        Assert.Equal(number, phone.ToString());
    }

    [Theory]
    [InlineData("673 921 4850")]
    [InlineData("673.921.4850")]
    [InlineData("673-921-4850")]
    public void Should_ConvertToString_When_PerformImplicitOperator(string number)
    {
        string phone = Phone.New(number);
        Assert.Equal(phone, number);
    }

    [Theory]
    [InlineData("673 921 4850")]
    [InlineData("673.921.4850")]
    [InlineData("673-921-4850")]
    public void Should_ConvertToPhone_When_PerformExplicitOperator(string number)
    {
        Phone phone = (Phone)number;
        Assert.Equal(phone, Phone.New(number));
    }

    [Theory]
    [InlineData("3 91 485")]
    [InlineData("673921485")]
    public void Should_ThrowArgumentException_When_NumberDoesNotFullfitPhonePattern(string number)
    {
        Assert.Throws<ArgumentException>(() => Phone.New(number));
    }

    [Fact]
    public void Should_ThrowArgumentNullException_When_NumberIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Phone.New(null));
    }
}