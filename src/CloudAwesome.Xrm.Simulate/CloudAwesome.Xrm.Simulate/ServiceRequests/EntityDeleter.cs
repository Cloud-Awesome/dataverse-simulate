using CloudAwesome.Xrm.Simulate.DataStores;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests;

public class EntityDeleter: IEntityDeleter
{
    public void MockRequest(IOrganizationService organizationService, 
        ISimulatorOptions? options = null)
    {
        organizationService.When(x => 
            x.Delete(Arg.Any<string>(), Arg.Any<Guid>()))
            .Do(x =>
            {
                var entityName = x.Arg<string>();
                var id = x.Arg<Guid>();
                
                var entity = MockedEntityDataStore.Instance.Data[entityName].SingleOrDefault(x => x.Id == id);
                MockedEntityDataStore.Instance.Data[entityName].Remove(entity);
            });
    }
}