using CloudAwesome.Xrm.Simulate.DataStores;
using Microsoft.Extensions.Logging;

namespace CloudAwesome.Xrm.Simulate.DataServices;

public class MockedTelemetryService
{
    public void Add(LogLevel logLevel, string message, params object[] args)
    {
        MockedTelemetryStore.Instance.Logs.Add(new TelemetryMock
        {
            LogLevel = logLevel,
            MessageFormat = message,
            Args = args
        });
    }
    
    public void Clear()
    {
        MockedTelemetryStore.Instance.Logs.Clear();
    }

    public List<TelemetryMock> Get()
    {
        return MockedTelemetryStore.Instance.Logs;
    }

    public List<TelemetryMock> Get(LogLevel logLevel)
    {
        return MockedTelemetryStore.Instance.Logs
            .Where(x => x.LogLevel == logLevel)
            .ToList();
    }

    public List<TelemetryMock> Get(string messageTemplate)
    {
        return MockedTelemetryStore.Instance.Logs
            .Where(x => x.MessageFormat == messageTemplate)
            .ToList();
    }


}