using CloudAwesome.Xrm.Simulate.DataStores;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Moq;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests;

public class EntityUpdater: IEntityUpdater
{
    public void MockRequest(IOrganizationService organizationService, 
        ISimulatorOptions? options = null)
    {
        /*
         * - Also need to set other system generated fields, such as modifiedon, modifiedby etc...
         * - Decide on what to set as modifiedby..? Initial set up of a system user in the data..?
         *      - Might be nice to support some sort of persona testing..?
         * -  Anything required with entity.RowVersion?
         * - How about entity.FormattedValues? And ExtensionData? KeyAttributes?
         */
        
        var systemTime = options?.ClockSimulator?.Now ?? DateTime.Now;
        
        Mock.Get(organizationService)
            .Setup(x => x.Update(It.IsAny<Entity>()))
            .Callback((Entity entity) =>
            {
                var entityToUpdate = MockedEntityDataStore.Instance.Data[entity.LogicalName]
                    .SingleOrDefault(x => x.Id == entity.Id);
                
                if (entityToUpdate != null)
                {
                    MockedEntityDataStore.Instance.Data[entity.LogicalName].Remove(entityToUpdate);
                }
                else
                {
                    // TODO - Handle if the entity doesn't exist in memory
                    //      - Check the exact exception that would be thrown in .gather
                    throw new InvalidOperationException("Record not found in database ...");
                }
                
                entity.Attributes[EntityConstants.ModifiedOn] = systemTime;
                
                // TODO - This won't work, just deleting and re-creating.
                //      - Need to loop through those attributes passed through,
                //      - As only 1 attribute could be updated in the message...  
                MockedEntityDataStore.Instance.Data[entity.LogicalName].Add(entity);
            });
    }

    public Entity Initialise(Entity entity)
    {
        return entity;
    }
}