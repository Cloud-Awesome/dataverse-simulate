using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using CloudAwesome.Xrm.Simulate.ServiceRequests;
using Microsoft.Xrm.Sdk;
using Moq;

namespace CloudAwesome.Xrm.Simulate;

public static class OrganisationServiceSimulator
{
    private static readonly IOrganizationService Service = Mock.Of<IOrganizationService>();

    private static readonly IEntityCreator EntityCreator = new EntityCreator();
    private static readonly IEntityUpdater EntityUpdater = new EntityUpdater();
    private static readonly IEntityRetriever EntityRetriever = new EntityRetriever();
    private static readonly IEntityMultipleRetriever EntityMultipleRetriever = new EntityMultipleRetriever();
    private static readonly IEntityDeleter EntityDeleter = new EntityDeleter();
    private static readonly IEntityAssociator EntityAssociator = new EntityAssociator();
    private static readonly IEntityDisassociator EntityDisassociator = new EntityDisassociator();

    private static readonly IOrganisationRequestExecutor OrganisationRequestExecutor =
        new OrganisationRequestExecutor();
    
    public static IOrganizationService Simulate(this IOrganizationService organizationService, 
        ISimulatorOptions? options = null)
    {
        EntityCreator.MockRequest(Service, options);
        EntityRetriever.MockRequest(Service, options);
        EntityMultipleRetriever.MockRequest(Service, options);
        EntityUpdater.MockRequest(Service, options);
        EntityDeleter.MockRequest(Service, options);
        EntityAssociator.MockRequest(Service, options);
        EntityDisassociator.MockRequest(Service, options);
        OrganisationRequestExecutor.MockRequest(Service, options);

        var dataService = new MockedEntityDataService();
        ConfigureAuthenticatedUser(options, dataService);
        SetSystemTime(options, dataService);
        
        return Service;
    }

    public static MockedEntityDataService Data(this IOrganizationService organizationService)
    {
        return new MockedEntityDataService();
    }

    private static void SetSystemTime(ISimulatorOptions? options, MockedEntityDataService dataService)
    {
        if (options?.ClockSimulator is not null)
        {
            dataService.SystemTime = options.ClockSimulator.Now;
        }
    }

    private static EntityReference ConfigureAuthenticatedUser(ISimulatorOptions? options, MockedEntityDataService dataService)
    {
        if (options?.AuthenticatedUser is not null)
        {
            dataService.Add(options.AuthenticatedUser);
            dataService.AuthenticatedUser = options.AuthenticatedUser.ToEntityReference();
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
        
        dataService.Add(user);
        dataService.AuthenticatedUser = user.ToEntityReference();
        return user.ToEntityReference();
    }
}