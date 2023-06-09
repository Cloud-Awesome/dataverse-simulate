using CloudAwesome.Xrm.Simulate.DataServices;
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
        
        Mock.Get(organizationService)
            .Setup(x => x.Update(It.IsAny<Entity>()))
            .Callback((Entity entity) =>
            {
                var dataService = new MockedEntityDataService();
                
                var e = MockedEntityDataStore.Instance.Data[entity.LogicalName]
                    .SingleOrDefault(x => x.Id == entity.Id);
                
                if (options?.EntityProcessors?.TryGetValue(e.LogicalName, 
                        out var entityProcessor) == true && 
                    entityProcessor.TryGetValue(SimulatorOptions.ProcessorMessage.Update, 
                        out var processor))
                {
                    processor.Process(e);
                }
                
                if (e != null)
                {
                    MockedEntityDataStore.Instance.Data[entity.LogicalName].Remove(e);
                }
                else
                {
                    // TODO - Handle if the entity doesn't exist in memory
                    //      - Check the exact exception that would be thrown in .gather
                    throw new InvalidOperationException("Record not found in database ...");
                }
                
                entity.Attributes[EntityConstants.ModifiedOn] = dataService.SystemTime;
                
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