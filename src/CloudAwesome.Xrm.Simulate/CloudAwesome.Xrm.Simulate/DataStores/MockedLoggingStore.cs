namespace CloudAwesome.Xrm.Simulate.DataStores;

internal sealed class MockedLoggingStore
{
    internal List<string> Logs { get; private set; } = new();
}