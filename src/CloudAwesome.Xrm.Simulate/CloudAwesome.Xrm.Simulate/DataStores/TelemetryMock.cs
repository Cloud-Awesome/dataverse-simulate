using Microsoft.Xrm.Sdk.PluginTelemetry;

namespace CloudAwesome.Xrm.Simulate.DataStores;

public class TelemetryMock
{
    public string MessageFormat { get; set; }
    public object[] Args { get; set; }
    public LogLevel LogLevel { get; set; }
    
    public string FormattedMessage => string.Format(MessageFormat, Args);
}