using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Infrastructure.Loggers;
using Microsoft.Extensions.Logging;
using Moq;

namespace BMJ.Authenticator.Infrastructure.UnitTests.Loggers;

public class AuthLoggerTests
{
    private readonly IAuthLogger _authLogger;
    private readonly Mock<ILogger<BMJAuthenticator>> _logger;

    public AuthLoggerTests()
    {
        _logger = new();
        _logger.Setup(l => l.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
        _authLogger = new AuthLogger(_logger.Object);
    }

    [Fact]
    public void ShouldLogDebug()
    {
        var exception = Record.Exception(() => _authLogger.Debug("Message template"));
        var exception2 = Record.Exception(() => _authLogger.Debug("Message template", "value1"));
        var exception3 = Record.Exception(() => _authLogger.Debug("Message template", "value1", "value2"));
        var exception4 = Record.Exception(() => _authLogger.Debug("Message template", "value1", "value2", "value3"));
        var exception5 = Record.Exception(() => _authLogger.Debug(new Exception(), "Message template"));
        var exception6 = Record.Exception(() => _authLogger.Debug(new Exception(), "Message template", "value1"));
        var exception7 = Record.Exception(() => _authLogger.Debug(new Exception(), "Message template", "value1", "value2"));
        var exception8 = Record.Exception(() => _authLogger.Debug(new Exception(), "Message template", "value1", "value2", "value3"));

        Assert.Null(exception);
        Assert.Null(exception2);
        Assert.Null(exception3);
        Assert.Null(exception4);
        Assert.Null(exception5);
        Assert.Null(exception6);
        Assert.Null(exception7);
        Assert.Null(exception8);
    }

    [Fact]
    public void ShouldLogError()
    {
        var exception = Record.Exception(() => _authLogger.Error("Message template"));
        var exception2 = Record.Exception(() => _authLogger.Error("Message template", "value1"));
        var exception3 = Record.Exception(() => _authLogger.Error("Message template", "value1", "value2"));
        var exception4 = Record.Exception(() => _authLogger.Error("Message template", "value1", "value2", "value3"));
        var exception5 = Record.Exception(() => _authLogger.Error(new Exception(), "Message template"));
        var exception6 = Record.Exception(() => _authLogger.Error(new Exception(), "Message template", "value1"));
        var exception7 = Record.Exception(() => _authLogger.Error(new Exception(), "Message template", "value1", "value2"));
        var exception8 = Record.Exception(() => _authLogger.Error(new Exception(), "Message template", "value1", "value2", "value3"));

        Assert.Null(exception);
        Assert.Null(exception2);
        Assert.Null(exception3);
        Assert.Null(exception4);
        Assert.Null(exception5);
        Assert.Null(exception6);
        Assert.Null(exception7);
        Assert.Null(exception8);
    }

    [Fact]
    public void ShouldLogCritical()
    {
        var exception = Record.Exception(() => _authLogger.Critical("Message template"));
        var exception2 = Record.Exception(() => _authLogger.Critical("Message template", "value1"));
        var exception3 = Record.Exception(() => _authLogger.Critical("Message template", "value1", "value2"));
        var exception4 = Record.Exception(() => _authLogger.Critical("Message template", "value1", "value2", "value3"));
        var exception5 = Record.Exception(() => _authLogger.Critical(new Exception(), "Message template"));
        var exception6 = Record.Exception(() => _authLogger.Critical(new Exception(), "Message template", "value1"));
        var exception7 = Record.Exception(() => _authLogger.Critical(new Exception(), "Message template", "value1", "value2"));
        var exception8 = Record.Exception(() => _authLogger.Critical(new Exception(), "Message template", "value1", "value2", "value3"));

        Assert.Null(exception);
        Assert.Null(exception2);
        Assert.Null(exception3);
        Assert.Null(exception4);
        Assert.Null(exception5);
        Assert.Null(exception6);
        Assert.Null(exception7);
        Assert.Null(exception8);
    }

    [Fact]
    public void ShouldLogInformation()
    {
        var exception = Record.Exception(() => _authLogger.Information("Message template"));
        var exception2 = Record.Exception(() => _authLogger.Information("Message template", "value1"));
        var exception3 = Record.Exception(() => _authLogger.Information("Message template", "value1", "value2"));
        var exception4 = Record.Exception(() => _authLogger.Information("Message template", "value1", "value2", "value3"));
        var exception5 = Record.Exception(() => _authLogger.Information(new Exception(), "Message template"));
        var exception6 = Record.Exception(() => _authLogger.Information(new Exception(), "Message template", "value1"));
        var exception7 = Record.Exception(() => _authLogger.Information(new Exception(), "Message template", "value1", "value2"));
        var exception8 = Record.Exception(() => _authLogger.Information(new Exception(), "Message template", "value1", "value2", "value3"));

        Assert.Null(exception);
        Assert.Null(exception2);
        Assert.Null(exception3);
        Assert.Null(exception4);
        Assert.Null(exception5);
        Assert.Null(exception6);
        Assert.Null(exception7);
        Assert.Null(exception8);
    }

    [Fact]
    public void ShouldLogWarning()
    {
        var exception = Record.Exception(() => _authLogger.Warning("Message template"));
        var exception2 = Record.Exception(() => _authLogger.Warning("Message template", "value1"));
        var exception3 = Record.Exception(() => _authLogger.Warning("Message template", "value1", "value2"));
        var exception4 = Record.Exception(() => _authLogger.Warning("Message template", "value1", "value2", "value3"));
        var exception5 = Record.Exception(() => _authLogger.Warning(new Exception(), "Message template"));
        var exception6 = Record.Exception(() => _authLogger.Warning(new Exception(), "Message template", "value1"));
        var exception7 = Record.Exception(() => _authLogger.Warning(new Exception(), "Message template", "value1", "value2"));
        var exception8 = Record.Exception(() => _authLogger.Warning(new Exception(), "Message template", "value1", "value2", "value3"));

        Assert.Null(exception);
        Assert.Null(exception2);
        Assert.Null(exception3);
        Assert.Null(exception4);
        Assert.Null(exception5);
        Assert.Null(exception6);
        Assert.Null(exception7);
        Assert.Null(exception8);
    }
}
