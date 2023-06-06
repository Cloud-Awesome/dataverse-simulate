using CloudAwesome.Xrm.Simulate.DataStores;
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
    /// Clears data created, updated or deleted in a test and resets to the original configuration 
    /// (Either an empty dataset, or initialised from base mock)
    /// </summary>
    public void Reinitialise()
    {
        /*
         * TODO - Clear and then reload stuff we want to keep
         *  - Reference to a user
         *  - BUs, Teams, Fiscal Periods, etc...
         */
        
        MockedEntityDataStore.Instance.Data.Clear();
    }

    public EntityReference AuthenticatedUser
    {
        get => MockedEntityDataStore.Instance.AuthenticatedUser;
        set => MockedEntityDataStore.Instance.AuthenticatedUser = value;
    }
}