using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Moq;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests;

public class EntityDisassociator: IEntityDisassociator
{
    public void MockRequest(IOrganizationService organizationService, 
        ISimulatorOptions? options = null)
    {
        Mock.Get(organizationService)
            .Setup(x => x.Disassociate(
                It.IsAny<string>(), It.IsAny<Guid>(), 
                It.IsAny<Relationship>(), It.IsAny<EntityReferenceCollection>()))
            .Callback((string entityName, Guid entityId, 
                Relationship relationship, EntityReferenceCollection relatedEntities) =>
            {
                // Implement Mock
                // Set entity.RelatedEntities? or probably entity.SetRelatedEntities?
            });
    }

    public Entity Initialise(Entity entity)
    {
        return entity;
    }
}