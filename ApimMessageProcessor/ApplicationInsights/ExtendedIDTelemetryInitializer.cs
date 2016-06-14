
namespace ApimMessageProcessor.ApplicationInsights
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    public class ExtendedIDTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Properties["ETW.ActivityID"] = "11111";
            telemetry.Context.Properties["E2ETrace.ActivityID"] = "22222";
        }
    }
}
