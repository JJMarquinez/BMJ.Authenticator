using System.Diagnostics;

namespace BMJ.Authenticator.Application.Common.Instrumentation;

public class Telemetry
{
    public static readonly string ServiceName = "BMJ.Authenticator";
    public static readonly string Version = "1.0.0";
    public static readonly ActivitySource Source = new(ServiceName, Version);
}
