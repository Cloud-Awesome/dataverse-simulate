using CloudAwesome.Xrm.Simulate.DataStores;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests;

public class EntityRetriever: IEntityRetriever
{
    public void MockRequest(IOrganizationService organizationService, 
        ISimulatorOptions? options = null)
    {
        Mock.Get(organizationService)
            .Setup(x => x.Retrieve(
                It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
            .Returns((string entityName, Guid id, ColumnSet columnSet) =>
            {
                Entity entity;
                if (columnSet.AllColumns)
                {
                    entity = MockedEntityDataStore.Instance.Data[entityName]
                        .SingleOrDefault(x => x.Id == id);    
                }
                else
                {
                    entity = MockedEntityDataStore.Instance.Data[entityName]
                        .Select(x =>
                        {
                            var e = new Entity(x.LogicalName) { Id = x.Id };
                            foreach (var column in columnSet.Columns)
                            {
                                e[column] = x[column];
                            }
                            return e;
                        })
                        .FirstOrDefault();
                }
                    
                return entity;
            });
    }

    public Entity Initialise(Entity entity)
    {
        return entity;
    }
}