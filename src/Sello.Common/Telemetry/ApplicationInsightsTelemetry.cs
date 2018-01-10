using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Sello.Common.Configuration.Interfaces;
using Sello.Common.Telemetry.Interfaces;

namespace Sello.Common.Telemetry
{
    public class ApplicationInsightsTelemetry : ITelemetry
    {
        private readonly TelemetryClient applicationInsightsTelemetryClient;
        
        public ApplicationInsightsTelemetry(IConfigurationProvider configurationProvider)
        {
            var instrumentationKey = configurationProvider.GetSetting("Telemetry.Key");

            TelemetryConfiguration.Active.DisableTelemetry = false;

            applicationInsightsTelemetryClient = new TelemetryClient
            {
                InstrumentationKey = instrumentationKey
            };
        }

        public void TrackEvent(string eventName, Dictionary<string, string> eventContext)
        {
            applicationInsightsTelemetryClient.TrackEvent(eventName, eventContext);
        }
    }
}