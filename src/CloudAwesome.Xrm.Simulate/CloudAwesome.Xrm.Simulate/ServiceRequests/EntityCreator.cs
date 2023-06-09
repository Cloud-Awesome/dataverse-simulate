﻿using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.DataStores;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Moq;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests;

public sealed class EntityCreator: IEntityCreator
{
    public void MockRequest(IOrganizationService organizationService, 
        ISimulatorOptions? options = null)
    {
        //var systemTime = options?.ClockSimulator?.Now ?? DateTime.Now;

        Mock.Get(organizationService)
            .Setup(x => x.Create(It.IsAny<Entity>()))
            .Returns((Entity e) => this.Create(e, options));
    }

    internal Guid Create(Entity e, ISimulatorOptions? options)
    {
        /*
         * Validate the entity first... (And decide on the correct Exception to throw if not)
         * Set state and status
         * Anything required with entity.RowVersion?
         * How about entity.FormattedValues? And ExtensionData? KeyAttributes?
         * Does the entity already exist with that GUID? Throw exception.
         * work through e.RelatedEntities
         * Set triggers if plugins are registered
         */
        
        // Pre-process
        e = this.PreProcess(e, options);
        
        // Custom processing
        var processorType = new ProcessorType(e.LogicalName, ProcessorMessage.Create);
        if (options?.EntityProcessors?.TryGetValue(processorType, out var processor) == true)
        {
            e = processor.Process(e);
        }

        // Submit to data store
        if (MockedEntityDataStore.Instance
            .Data.TryGetValue(e.LogicalName, out var value))
        {
            value.Add(e);
        }
        else
        {
            MockedEntityDataStore.Instance
                .Data.Add(e.LogicalName, new List<Entity>() { e });
        }

        return e.Id;
    }
    
    internal Entity PreProcess(Entity e, ISimulatorOptions? options)
    {
        var dataService = new MockedEntityDataService();

        e.SetAttributeIfEmpty($"{e.LogicalName}id", Guid.NewGuid());
        e.Id = (Guid)e.Attributes[$"{e.LogicalName}id"];
        
        e.SetAttributeIfEmpty(EntityConstants.CreatedOn, dataService.SystemTime);
        e.SetAttributeFromSourceIfPopulated(EntityConstants.CreatedOn, 
            EntityConstants.OverridenCreatedOn);
        e.SetAttributeIfEmpty(EntityConstants.ModifiedOn, dataService.SystemTime);
        
        e.SetAttributeIfEmpty(EntityConstants.CreatedBy, dataService.AuthenticatedUser);
        e.SetAttributeIfEmpty(EntityConstants.ModifiedBy, dataService.AuthenticatedUser);
        e.SetAttributeIfEmpty(EntityConstants.OwnerId, dataService.AuthenticatedUser);

        return e;
    }
}