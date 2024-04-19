using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests;

public class EntityDeleter(MockedEntityDataService dataService) : IEntityDeleter
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
                
                dataService.Delete(entityName, id);
            });
    }
}