namespace CloudAwesome.Xrm.Simulate.DataStores;

public class MockedTelemetryStore
{
    private static readonly Lazy<MockedTelemetryStore> _instance =
        new Lazy<MockedTelemetryStore>(() => new MockedTelemetryStore());

    public static MockedTelemetryStore Instance => _instance.Value;

    public List<TelemetryMock> Logs { get; private set; }

    private MockedTelemetryStore()
    {
        Logs = new List<TelemetryMock>();
    }
}