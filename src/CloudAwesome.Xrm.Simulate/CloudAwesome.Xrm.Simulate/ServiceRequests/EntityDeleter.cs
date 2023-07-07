using CloudAwesome.Xrm.Simulate.DataStores;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Moq;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests;

public class EntityDeleter: IEntityDeleter
{
    public void MockRequest(IOrganizationService organizationService, 
        ISimulatorOptions? options = null)
    {
        Mock.Get(organizationService)
            .Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<Guid>()))
            .Callback((string entityName, Guid id) =>
            {
                /*
                 * Handle the types of validation and exceptions that CRM would throw
                 */
                var entity = MockedEntityDataStore.Instance.Data[entityName].SingleOrDefault(x => x.Id == id);
                MockedEntityDataStore.Instance.Data[entityName].Remove(entity);
            });
    }
}