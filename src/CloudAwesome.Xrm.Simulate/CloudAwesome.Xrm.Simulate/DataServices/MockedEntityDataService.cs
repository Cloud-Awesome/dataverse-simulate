using CloudAwesome.Xrm.Simulate.DataStores;
using CloudAwesome.Xrm.Simulate.Interfaces;
using CloudAwesome.Xrm.Simulate.ServiceProviders;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.DataServices;

public class MockedEntityDataService
{
    /// <summary>
    /// Initialise the in memory store with multiple entities and records.
    /// This overwrites any previously set data
    /// </summary>
    /// <param name="data"></param>
    public void Add(Dictionary<string, List<Entity>> data)
    {
        MockedEntityDataStore.Instance.Set(data);
    }
    
    /// <summary>
    /// Add a single entity record to the in memory store
    /// </summary>
    /// <param name="entity"></param>
    public void Add(Entity entity)
    {
        if (MockedEntityDataStore.Instance.Data
            .TryGetValue(entity.LogicalName, out var value))
        {
            value.Add(entity);
        }
        else
        {
            MockedEntityDataStore.Instance.Data.Add(entity.LogicalName, new List<Entity>{ entity });
        }
    }

    /// <summary>
    /// Get all data currently saved in the in memory store
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, List<Entity>> Get()
    {
        return MockedEntityDataStore.Instance.Data;
    }

    /// <summary>
    /// Get all data for a specific entity
    /// </summary>
    /// <param name="logicalName">The schema name of the entity's data to retrieve</param>
    /// <returns></returns>
    public List<Entity> Get(string logicalName)
    {
        MockedEntityDataStore.Instance.Data.TryGetValue(logicalName, out var entities);
        return entities ?? new List<Entity>();
    }

    /// <summary>
    /// Clears all data created, updated or deleted in a test and resets the in memory database to an empty set
    /// </summary>
    public void Reinitialise()
    {
        MockedEntityDataStore.Instance.Data.Clear();
    }
    
    /// <summary>
    /// Clears data created, updated or deleted in a test and resets to the original configuration,
    /// including reference data set up, such as Business Unit, Fiscal Periods, etc. 
    /// </summary>
    /// <param name="options"></param>
    public void Reinitialise(ISimulatorOptions options)
    {
        /*
         * TODO - Clear and then reload stuff we want to keep
         *  - Reference to a user
         *  - BUs, Teams, Fiscal Periods, etc...
         */
        
        MockedEntityDataStore.Instance.Data.Clear();
    }

    /// <summary>
    /// Resets the plugin execution context while maintaining all in memory data and configuration
    /// </summary>
    /// <param name="executionContextMock">Mocked plugin execution context, e.g. Target entity, Entity images</param>
    public void Reinitialise(PluginExecutionContextMock executionContextMock)
    {
        // TODO - What else needs doing to trigger this? It needs to get into the service provider/context mock
        MockedEntityDataStore.Instance.ExecutionContextMock = executionContextMock;
    }

    /// <summary>
    /// Contains a reference to the mocked SystemUser authenticated in to Dynamics.
    /// This will be used as the reference for metadata such as CreatedBy, ModifiedBy, etc.
    /// </summary>
    public EntityReference AuthenticatedUser
    {
        get => MockedEntityDataStore.Instance.AuthenticatedUser;
        set => MockedEntityDataStore.Instance.AuthenticatedUser = value;
    }

    /// <summary>
    /// Stores a mocked DateTime used as the Dynamics system time.
    /// This will be used for CreatedOn, ModifiedOn, etc. 
    /// as well as the basis for datetime filters and queries (e.g. Today, LastYear)
    /// </summary>
    public DateTime SystemTime
    {
        get => MockedEntityDataStore.Instance.SystemTime;
        internal set => MockedEntityDataStore.Instance.SystemTime = value;
    }

    public PluginExecutionContextMock? ExecutionContext
    {
        get => MockedEntityDataStore.Instance.ExecutionContextMock;
        set => MockedEntityDataStore.Instance.ExecutionContextMock = value?? new PluginExecutionContextMock();
    }

    public Entity BusinessUnit => throw new NotImplementedException();
    public EntityReference Organization => throw new NotImplementedException();

    public FakeServiceFailureSettings? FakeServiceFailureSettings
    {
        get => MockedEntityDataStore.Instance.FakeServiceFailureSettings;
        set => MockedEntityDataStore.Instance.FakeServiceFailureSettings = value?? new FakeServiceFailureSettings();
    }
}