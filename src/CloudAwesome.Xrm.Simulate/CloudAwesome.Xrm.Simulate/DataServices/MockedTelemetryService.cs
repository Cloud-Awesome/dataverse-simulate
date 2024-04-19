using CloudAwesome.Xrm.Simulate.DataStores;
using Microsoft.Xrm.Sdk.PluginTelemetry;

namespace CloudAwesome.Xrm.Simulate.DataServices;

public class MockedTelemetryService
{
    private readonly MockedTelemetryStore _telemetryStore = new(); 
    
    public void Add(LogLevel logLevel, string message, params object[] args)
    {
        _telemetryStore.Logs.Add(new TelemetryMock
        {
            LogLevel = logLevel,
            MessageFormat = message,
            Args = args
        });
    }
    
    public void Clear()
    {
        _telemetryStore.Logs.Clear();
    }

    public List<TelemetryMock> Get()
    {
        return _telemetryStore.Logs;
    }

    public List<TelemetryMock> Get(LogLevel logLevel)
    {
        return _telemetryStore.Logs
            .Where(x => x.LogLevel == logLevel)
            .ToList();
    }

    public List<TelemetryMock> Get(string messageTemplate)
    {
        return _telemetryStore.Logs
            .Where(x => x.MessageFormat == messageTemplate)
            .ToList();
    }


}