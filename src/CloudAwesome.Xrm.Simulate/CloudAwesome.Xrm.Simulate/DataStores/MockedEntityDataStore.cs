using CloudAwesome.Xrm.Simulate.ServiceProviders;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.DataStores;

internal sealed class MockedEntityDataStore
{
    private static readonly Lazy<MockedEntityDataStore> _instance =
        new Lazy<MockedEntityDataStore>(() => new MockedEntityDataStore());

    public static MockedEntityDataStore Instance => _instance.Value;

    public Dictionary<string, List<Entity>> Data { get; private set; }

    public EntityReference AuthenticatedUser { get; internal set; } = new EntityReference("systemuser", Guid.NewGuid());
    
    public DateTime SystemTime { get; internal set; } = DateTime.Now;
    
    public PluginExecutionContextMock ExecutionContextMock { get; internal set; }
    
    public FakeServiceFailureSettings FakeServiceFailureSettings { get; internal set; }
    
    private MockedEntityDataStore()
    {
        Data = new Dictionary<string, List<Entity>>();
    }

    internal void Set(Dictionary<string, List<Entity>> data)
    {
        Data = data;
    }
}
