using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.DataStores;

internal sealed class MockedEntityDataStore
{
    private static readonly Lazy<MockedEntityDataStore> _instance =
        new Lazy<MockedEntityDataStore>(() => new MockedEntityDataStore());

    public static MockedEntityDataStore Instance => _instance.Value;

    public Dictionary<string, List<Entity>> Data { get; private set; }

    public EntityReference AuthenticatedUser { get; internal set; } = null!;
    
    private MockedEntityDataStore()
    {
        Data = new Dictionary<string, List<Entity>>();
    }

    public void Set(Dictionary<string, List<Entity>> data)
    {
        Data = data;
    }
}
