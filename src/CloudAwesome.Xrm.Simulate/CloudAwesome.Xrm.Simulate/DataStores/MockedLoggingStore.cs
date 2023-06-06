namespace CloudAwesome.Xrm.Simulate.DataStores;

internal sealed class MockedLoggingStore
{
    private static readonly Lazy<MockedLoggingStore> _instance =
        new Lazy<MockedLoggingStore>(() => new MockedLoggingStore());

    internal static MockedLoggingStore Instance => _instance.Value;

    internal List<string> Logs { get; private set; }

    private MockedLoggingStore()
    {
        Logs = new List<string>();
    }
}