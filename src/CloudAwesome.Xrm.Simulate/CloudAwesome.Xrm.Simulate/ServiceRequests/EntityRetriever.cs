using CloudAwesome.Xrm.Simulate.DataStores;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests;

public class EntityRetriever: IEntityRetriever
{
    public void MockRequest(IOrganizationService organizationService, 
        ISimulatorOptions? options = null)
    {
        organizationService.Retrieve(Arg.Any<string>(), Arg.Any<Guid>(), Arg.Any<ColumnSet>())
            .Returns(x =>
            {
                var entityName = x.Arg<string>();
                var id = x.Arg<Guid>();
                var columnSet = x.Arg<ColumnSet>();
                
                if (!MockedEntityDataStore.Instance.Data.ContainsKey(entityName))
                {
                    // TODO - Confirm the exception thrown by live CRM
                    throw new InvalidOperationException("No data for this entity");
                }
                
                Entity entity;
                if (columnSet.AllColumns)
                {
                    // TODO - Confirm the exception thrown by live CRM when record not found
                    entity = MockedEntityDataStore.Instance.Data[entityName]
                                 .SingleOrDefault(x => x.Id == id) 
                             ?? throw new InvalidOperationException("No data for this entity");
                }
                else
                {
                    // TODO - Confirm the exception thrown by live CRM when record not found
                    entity = MockedEntityDataStore.Instance.Data[entityName]
                         .Select(x =>
                         {
                             var e = new Entity(x.LogicalName) { Id = x.Id };
                             foreach (var column in columnSet.Columns)
                             {
                                 e[column] = x[column];
                             }

                             // Always return the primary GUID, even if it's not requested
                             e[$"{x.LogicalName}id"] = x.Id; 
                    
                             return e;
                         })
                         .FirstOrDefault() 
                     ?? throw new InvalidOperationException("No data for this entity");
                }
                    
                return entity;
            });
    }
}