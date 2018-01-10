using System;
using System.Collections.Generic;

namespace Sello.Common.Telemetry.Interfaces
{
    public interface ITelemetry
    {
        void TrackEvent(string eventName, Dictionary<string, string> eventContext);
        void TrackException(Exception exception);
    }
}