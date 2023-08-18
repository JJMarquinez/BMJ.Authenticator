using BMJ.Authenticator.Domain.ValueObjects;

namespace BMJ.Authenticator.Domain.UnitTests.Common;

public class ValueObjectTests
{
    [Fact]
    public void ShouldBeEqualGivenSameValueObject()
    {
        Phone firstPhone = Phone.New("111 222 3333");
        Phone secondPhone = Phone.New("111 222 3333");
        Assert.True(firstPhone == secondPhone);
    }

    [Fact]
    public void ShouldNotBeEqualGivenNullAsSecondValueObject()
    {   
        Phone firstPhone = Phone.New("111 222 3333");
        Phone secondPhone = null!;
        Assert.False(firstPhone == secondPhone);
    }

    [Fact]
    public void ShouldNotBeEqualGivenNullAsFirstValueObject()
    {
        Phone firstPhone = null!;
        Phone secondPhone = Phone.New("111 222 3333");
        Assert.False(firstPhone == secondPhone);
    }

    [Fact]
    public void ShouldBeEqualGivenNullsToCampreWith()
    {
        Phone firstPhone = null!;
        Phone secondPhone = null!;
        Assert.True(firstPhone == secondPhone);
    }

    [Fact]
    public void ShouldNotBeDifferentGivenSameValueObjects()
    {
        Phone firstPhone = Phone.New("111 222 3333");
        Phone secondPhone = Phone.New("111 222 3333");
        Assert.False(firstPhone != secondPhone);
    }

    [Fact]
    public void ShouldBeDifferentGivenNullAsSecondValueObject()
    {
        Phone firstPhone = Phone.New("111 222 3333");
        Phone secondPhone = null!;
        Assert.True(firstPhone != secondPhone);
    }

    [Fact]
    public void ShouldBeDifferentGivenNullAsFirstValueObject()
    {
        Phone firstPhone = null!;
        Phone secondPhone = Phone.New("111 222 3333");
        Assert.True(firstPhone != secondPhone);
    }

    [Fact]
    public void ShouldNotBeDifferentGivenNullsToCompareWith()
    {
        Phone firstPhone = null!;
        Phone secondPhone = null!;
        Assert.False(firstPhone != secondPhone);
    }
}
