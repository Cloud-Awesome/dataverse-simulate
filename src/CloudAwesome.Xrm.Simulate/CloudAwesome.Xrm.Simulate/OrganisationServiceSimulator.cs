using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using CloudAwesome.Xrm.Simulate.ServiceRequests;
using Microsoft.Xrm.Sdk;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate;

public static class OrganisationServiceSimulator
{
    private static readonly MockedEntityDataService DataService = new();
    private static readonly SimulatorAuditService AuditService = new();
    
    private static readonly IOrganizationService Service = Substitute.For<IOrganizationService>();
    
    private static readonly IEntityCreator EntityCreator = new EntityCreator(DataService, AuditService);
    private static readonly IEntityUpdater EntityUpdater = new EntityUpdater(DataService);
    private static readonly IEntityRetriever EntityRetriever = new EntityRetriever(DataService, AuditService);
    private static readonly IEntityMultipleRetriever EntityMultipleRetriever = new EntityMultipleRetriever(DataService);
    private static readonly IEntityDeleter EntityDeleter = new EntityDeleter(DataService);
    private static readonly IEntityAssociator EntityAssociator = new EntityAssociator(DataService);
    private static readonly IEntityDisassociator EntityDisassociator = new EntityDisassociator(DataService);

    private static readonly IOrganisationRequestExecutor OrganisationRequestExecutor =
        new OrganisationRequestExecutor(DataService, AuditService);
    
    public static IOrganizationService Simulate(this IOrganizationService organizationService, 
        ISimulatorOptions? options = null)
    {
        DataService.Reinitialise();
        AuditService.Clear();
        
        EntityCreator.MockRequest(Service, options);
        EntityRetriever.MockRequest(Service, options);
        EntityMultipleRetriever.MockRequest(Service, options);
        EntityUpdater.MockRequest(Service, options);
        EntityDeleter.MockRequest(Service, options);
        EntityAssociator.MockRequest(Service, options);
        EntityDisassociator.MockRequest(Service, options);
        OrganisationRequestExecutor.MockRequest(Service, options);

        ConfigureAuthenticatedUser(options);
        SetSystemTime(options);
        
        return Service;
    }

    public static OrganisationServiceSimulated Simulated(this IOrganizationService organizationService)
    {
        return new OrganisationServiceSimulated(DataService, AuditService);
    }

    private static void SetSystemTime(ISimulatorOptions? options)
    {
        if (options?.ClockSimulator is not null)
        {
            DataService.SystemTime = options.ClockSimulator.Now;
        }
    }

    private static EntityReference ConfigureAuthenticatedUser(ISimulatorOptions? options)
    {
        if (options?.AuthenticatedUser is not null)
        {
            DataService.Add(options.AuthenticatedUser);
            DataService.AuthenticatedUser = options.AuthenticatedUser.ToEntityReference();
            return options.AuthenticatedUser.ToEntityReference();    
        }
        
        var user = new Entity("systemuser")
        {
            Id = Guid.NewGuid(),
            Attributes =
            {
                ["fullname"] = "Simulated User"
            }
        };
        
        DataService.Add(user);
        DataService.AuthenticatedUser = user.ToEntityReference();
        return user.ToEntityReference();
    }
}