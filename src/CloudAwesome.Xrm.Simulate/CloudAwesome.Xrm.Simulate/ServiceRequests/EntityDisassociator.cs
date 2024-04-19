using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests;

public class EntityDisassociator(MockedEntityDataService dataService): IEntityDisassociator
{
    public void MockRequest(IOrganizationService organizationService, 
        ISimulatorOptions? options = null)
    {
        organizationService.When(x => 
            x.Disassociate(Arg.Any<string>(), Arg.Any<Guid>(), 
                Arg.Any<Relationship>(), Arg.Any<EntityReferenceCollection>()))
            .Do(x =>
            {
                // Implement Mock
                // Set entity.RelatedEntities? or probably entity.SetRelatedEntities?
            });
    }
}