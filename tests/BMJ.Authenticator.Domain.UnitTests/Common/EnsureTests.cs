using BMJ.Authenticator.Domain.Common;

namespace BMJ.Authenticator.Domain.UnitTests.Common;

public class EnsureTests
{
    [Fact]
    public void ShouldNotThrowExceptionGivenTrueCondition()
    {
        var condition = true;
        var exceptionMessage = "The exception is due to the condition.";
        var exception = Record.Exception(() => Ensure.That(condition, exceptionMessage));
        Assert.Null(exception);
    }

    [Fact]
    public void ShouldThrowExceptionGivenFalseCondition()
    {
        var condition = false;
        var exceptionMessage = "The exception is due to the condition.";
        var exception = Record.Exception(() => Ensure.That(condition, exceptionMessage));
        Assert.NotNull(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Fact]
    public void ShouldNotThrowExceptionGivenFalseCondition()
    {
        var condition = false;
        var exceptionMessage = "The exception is due to the condition.";
        var exception = Record.Exception(() => Ensure.Not(condition, exceptionMessage));
        Assert.Null(exception);
    }

    [Fact]
    public void ShouldThrowExceptionGivenTrueCondition()
    {
        var condition = true;
        var exceptionMessage = "The exception is due to the condition.";
        var exception = Record.Exception(() => Ensure.Not(condition, exceptionMessage));
        Assert.NotNull(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Fact]
    public void ShouldNotThrowExceptionGivenNotNullObject()
    {
        var condition = new object();
        var exceptionMessage = "The exception is due to the condition.";
        var exception = Record.Exception(() => Ensure.NotNull(condition, exceptionMessage));
        Assert.Null(exception);
    }

    [Fact]
    public void ShouldThrowExceptionGivenNullObject()
    {
        object condition = null!;
        var exceptionMessage = "The exception is due to the condition.";
        var exception = Record.Exception(() => Ensure.NotNull(condition, exceptionMessage));
        Assert.NotNull(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Fact]
    public void ShouldNotThrowExceptionGivenNotNullOrEmptyString()
    {
        var condition = "value string";
        var exceptionMessage = "The exception is due to the condition.";
        var exception = Record.Exception(() => Ensure.NotNullOrEmpty(condition, exceptionMessage));
        Assert.Null(exception);
    }

    [Fact]
    public void ShouldThrowExceptionGivenNullOrEmptyString()
    {
        var condition = string.Empty;
        var exceptionMessage = "The exception is due to the condition.";
        var exception = Record.Exception(() => Ensure.NotNullOrEmpty(condition, exceptionMessage));
        Assert.NotNull(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Fact]
    public void ShouldNotThrowExceptionGivenSameObjects()
    {
        var stringA = "value string";
        var stringB = "value string";
        var exceptionMessage = "The exception is due to the condition.";
        var exception = Record.Exception(() => Ensure.Equal(stringA, stringB, exceptionMessage));
        Assert.Null(exception);
    }

    [Fact]
    public void ShouldThrowExceptionGivenDifferentObjects()
    {
        var stringA = "value string";
        string stringB = null!;
        var exceptionMessage = "The exception is due to the condition.";
        var exception = Record.Exception(() => Ensure.Equal(stringA, stringB, exceptionMessage));
        Assert.NotNull(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Fact]
    public void ShouldNotThrowExceptionGivenDifferentObjects()
    {
        var stringA = "value string";
        string stringB = "key string";
        var exceptionMessage = "The exception is due to the condition.";
        var exception = Record.Exception(() => Ensure.NotEqual(stringA, stringB, exceptionMessage));
        Assert.Null(exception);
    }

    [Fact]
    public void ShouldThrowExceptionGivenSameObjects()
    {
        var stringA = "value string";
        var stringB = "value string";
        var exceptionMessage = "The exception is due to the condition.";
        var exception = Record.Exception(() => Ensure.NotEqual(stringA, stringB, exceptionMessage));
        Assert.NotNull(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Fact]
    public void ShouldThrowExceptionGivenDifferentObjectsButNullOfThem()
    {
        var stringA = "value string";
        string stringB = null!;
        var exceptionMessage = "The exception is due to the condition.";
        var exception = Record.Exception(() => Ensure.NotEqual(stringA, stringB, exceptionMessage));
        Assert.NotNull(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Fact]
    public void ShouldNotThrowExceptionGivenValueInCollection()
    {
        var value = "value string";
        var collection = new List<string>() { value };
        var exceptionMessage = "The exception is due to the condition.";
        var exception = Record.Exception(() => Ensure.Contains(collection, v => v.Equals(value), exceptionMessage));
        Assert.Null(exception);
    }

    [Fact]
    public void ShouldThrowExceptionGivenNotValueInCollection()
    {
        var valueA = "value string";
        var valueB = "key string";
        var collection = new List<string>() { valueA };
        var exceptionMessage = "The exception is due to the condition.";
        var exception = Record.Exception(() => Ensure.Contains(collection, v => v.Equals(valueB), exceptionMessage));
        Assert.NotNull(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Fact]
    public void ShouldThrowExceptionGivenNullCollection()
    {
        var value = "value string";
        IEnumerable<string> collection = null!;
        var exceptionMessage = "The exception is due to the condition.";
        var exception = Record.Exception(() => Ensure.Contains(collection, v => v.Equals(value), exceptionMessage));
        Assert.NotNull(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Fact]
    public void ShouldNotThrowExceptionGivenAllValueFullfilingThePredicateInCollection()
    {
        var value = "value string";
        var collection = new List<string>() { value };
        var exceptionMessage = "The exception is due to the condition.";
        var exception = Record.Exception(() => Ensure.Items(collection, v => !string.IsNullOrEmpty(v), exceptionMessage));
        Assert.Null(exception);
    }

    [Fact]
    public void ShouldThrowExceptionGivenAllValueNotFullfilingThePredicateInCollection()
    {
        var value = string.Empty;
        var collection = new List<string>() { value };
        var exceptionMessage = "The exception is due to the condition.";
        var exception = Record.Exception(() => Ensure.Items(collection, v => !string.IsNullOrEmpty(v), exceptionMessage));
        Assert.NotNull(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Fact]
    public void ShouldThrowExceptionGivenNullCollectionCallingItemMethod()
    {
        var value = string.Empty;
        IEnumerable<string> collection = null!;
        var exceptionMessage = "The exception is due to the condition.";
        var exception = Record.Exception(() => Ensure.Items(collection, v => !string.IsNullOrEmpty(v), exceptionMessage));
        Assert.NotNull(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }
}
