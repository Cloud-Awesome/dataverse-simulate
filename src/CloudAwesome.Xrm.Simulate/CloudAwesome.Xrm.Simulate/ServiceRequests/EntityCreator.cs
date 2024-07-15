﻿using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.DataStores;
using CloudAwesome.Xrm.Simulate.Interfaces;
using CloudAwesome.Xrm.Simulate.SecurityModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests;

public sealed class EntityCreator
    (MockedEntityDataService dataService, SimulatorAuditService auditService) : IEntityCreator
{
    private const string RequestMessage = "Create";
    
    public void MockRequest(IOrganizationService organizationService, 
        ISimulatorOptions? options = null)
    {
        organizationService.Create(Arg.Any<Entity>())
            .Returns(x =>
            {
                var entity = x.Arg<Entity>();
                return this.Create(entity, options);
            });
    }

    internal Guid Create(Entity e, ISimulatorOptions? options)
    {
        if (!PermissionsCalculator.ValidateEntityPermission(e.LogicalName, RequestMessage, options))
        {
            // Exactly which type of error is thrown by crm?
            throw new Exception("Tester...");
        }
        
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
        dataService.Add(e);
        auditService.Add(RequestMessage, e.LogicalName, e.Id);
        
        return e.Id;
    }
    
    internal Entity PreProcess(Entity e, ISimulatorOptions? options)
    {
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