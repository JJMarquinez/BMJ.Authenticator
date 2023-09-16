using BMJ.Authenticator.Application.Common.Instrumentation;

namespace BMJ.Authenticator.Application.UnitTests.Common.Instrumentation;

public class TelemetryTests
{
    [Fact]
    public void ShouldValidateTelemetryData()
    {
        var serviceName = "BMJ.Authenticator";
        var serviceVersion = "1.0.0";
        Assert.Equal(serviceName, Telemetry.ServiceName);
        Assert.Equal(serviceVersion, Telemetry.Version);
        Assert.NotNull(Telemetry.Source);
        Assert.Equal(serviceName, Telemetry.Source.Name);
        Assert.Equal(serviceVersion, Telemetry.Source.Version);
    }
}
