using BMJ.Authenticator.Domain.ValueObjects;

namespace BMJ.Authenticator.Domain.UnitTests.Common;

public class ValueObjectTests
{
    [Fact]
    public void Should_BeEqual_When_TheyAreTheSame()
    {
        Phone firstPhone = Phone.New("111 222 3333");
        Phone secondPhone = Phone.New("111 222 3333");
        Assert.True(firstPhone == secondPhone);
    }

    [Fact]
    public void Should_NotBeEqual_When_TheRightOneOfThemIsNull()
    {   
        Phone firstPhone = Phone.New("111 222 3333");
        Phone secondPhone = null!;
        Assert.False(firstPhone == secondPhone);
    }

    [Fact]
    public void Should_NotBeEqual_When_TheLeftOneOfThemIsNull()
    {
        Phone firstPhone = null!;
        Phone secondPhone = Phone.New("111 222 3333");
        Assert.False(firstPhone == secondPhone);
    }

    [Fact]
    public void Should_BeEqual_When_BothOfThemAreNull()
    {
        Phone firstPhone = null!;
        Phone secondPhone = null!;
        Assert.True(firstPhone == secondPhone);
    }

    [Fact]
    public void Should_NotBeDifferent_When_TheyAreTheSame()
    {
        Phone firstPhone = Phone.New("111 222 3333");
        Phone secondPhone = Phone.New("111 222 3333");
        Assert.False(firstPhone != secondPhone);
    }

    [Fact]
    public void Should_BeDifferent_When_TheRightOneOfThemIsNull()
    {
        Phone firstPhone = Phone.New("111 222 3333");
        Phone secondPhone = null!;
        Assert.True(firstPhone != secondPhone);
    }

    [Fact]
    public void Should_BeDifferent_When_TheLeftOneOfThemIsNull()
    {
        Phone firstPhone = null!;
        Phone secondPhone = Phone.New("111 222 3333");
        Assert.True(firstPhone != secondPhone);
    }

    [Fact]
    public void Should_NotBeDifferent_When_BothOfThemAreNull()
    {
        Phone firstPhone = null!;
        Phone secondPhone = null!;
        Assert.False(firstPhone != secondPhone);
    }
}
