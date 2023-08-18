using BMJ.Authenticator.Domain.ValueObjects;

namespace BMJ.Authenticator.Domain.UnitTests.ValueObjects;

public class PhoneTests
{
    [Theory]
    [InlineData("673 921 4850")]
    [InlineData("673.921.4850")]
    [InlineData("673-921-4850")]
    public void ShouldCreateAPhoneGivenValidNumber(string number)
    {
        Phone phone = Phone.New(number);
        Assert.Equal(number, phone.ToString());
    }

    [Theory]
    [InlineData("673 921 4850")]
    [InlineData("673.921.4850")]
    [InlineData("673-921-4850")]
    public void ShouldConvertToString(string number)
    {
        string phone = Phone.New(number);
        Assert.Equal(phone, number);
    }

    [Theory]
    [InlineData("673 921 4850")]
    [InlineData("673.921.4850")]
    [InlineData("673-921-4850")]
    public void ShouldConvertToPhone(string number)
    {
        Phone phone = (Phone)number;
        Assert.Equal(phone, Phone.New(number));
    }

    [Theory]
    [InlineData("3 91 485")]
    [InlineData("673921485")]
    public void ShouldThrowArgumentExceptionGivenInvalidNumber(string number)
    {
        Assert.Throws<ArgumentException>(() => Phone.New(number));
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenNullAsNumber()
    {
        Assert.Throws<ArgumentNullException>(() => Phone.New(null!));
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionGivenEmptyStringAsNumber()
    {
        Assert.Throws<ArgumentException>(() => Phone.New(string.Empty));
    }

    [Fact]
    public void ShouldPhoneBeDifferenceGivenADistincObject()
    {
        Assert.False(Phone.New("584-932-6789").Equals(new object()));
    }

    [Fact]
    public void ShouldPhoneBeDifferenceGivenNullToCompareWith()
    {
        Assert.False(Phone.New("584-932-6789").Equals(null));
    }

    [Fact]
    public void ShouldCreatePhone()
    {
        Assert.NotNull(Phone.New("584-932-6789"));
    }
}