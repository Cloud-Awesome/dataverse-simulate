namespace CloudAwesome.Xrm.Simulate.DataStores;

public class MockedTelemetryStore
{
    public List<TelemetryMock> Logs { get; private set; } = new();
}