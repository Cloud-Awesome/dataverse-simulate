using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests;

public class EntityAssociator: IEntityAssociator
{
    public void MockRequest(IOrganizationService organizationService, 
        ISimulatorOptions? options = null)
    {
        organizationService.When(x => 
            x.Associate(Arg.Any<string>(), Arg.Any<Guid>(),
                Arg.Any<Relationship>(), Arg.Any<EntityReferenceCollection>()))
            .Do(x =>
            {
                // Implement Mock
                // Set entity.RelatedEntities? or probably entity.SetRelatedEntities?
            });
    }
}