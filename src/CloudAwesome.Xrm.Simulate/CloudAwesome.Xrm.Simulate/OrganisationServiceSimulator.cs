using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using CloudAwesome.Xrm.Simulate.ServiceRequests;
using Microsoft.Xrm.Sdk;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate;

public static class OrganisationServiceSimulator
{
    private static MockedEntityDataService _dataService = new();
    private static readonly SimulatorAuditService AuditService = new();
    
    private static readonly IOrganizationService Service = Substitute.For<IOrganizationService>();
    
    public static IOrganizationService Simulate(this IOrganizationService organizationService, 
        ISimulatorOptions? options = null, MockedEntityDataService? dataService = null)
    {
        PassThroughDataService(dataService);
        
        _dataService.Reinitialise();
        AuditService.Clear();

        new EntityCreator(_dataService, AuditService).MockRequest(Service, options);
        new EntityRetriever(_dataService, AuditService).MockRequest(Service, options);
        new EntityMultipleRetriever(_dataService).MockRequest(Service, options);
        new EntityUpdater(_dataService).MockRequest(Service, options);
        new EntityDeleter(_dataService).MockRequest(Service, options);
        new EntityAssociator(_dataService).MockRequest(Service, options);
        new EntityDisassociator(_dataService).MockRequest(Service, options);
        new OrganisationRequestExecutor(_dataService, AuditService).MockRequest(Service, options);
        
        InitialiseMockedData(options);
        ConfigureAuthenticatedUser(options);
        SetSystemTime(options);
        
        return Service;
    }

    public static OrganisationServiceSimulated Simulated(this IOrganizationService organizationService)
    {
        return new OrganisationServiceSimulated(_dataService, AuditService);
    }

    private static void SetSystemTime(ISimulatorOptions? options)
    {
        if (options?.ClockSimulator is not null)
        {
            _dataService.SystemTime = options.ClockSimulator.Now;
        }
    }

    private static void InitialiseMockedData(ISimulatorOptions? options)
    {
        if (options?.InitialiseData is not null)
        {
            _dataService.Add(options.InitialiseData);
        }
    }

    private static void PassThroughDataService(MockedEntityDataService? dataService)
    {
        if (dataService is not null)
        {
            _dataService = dataService;
        }
    }

    private static EntityReference ConfigureAuthenticatedUser(ISimulatorOptions? options)
    {
        if (options?.AuthenticatedUser is not null)
        {
            _dataService.Add(options.AuthenticatedUser);
            _dataService.AuthenticatedUser = options.AuthenticatedUser.ToEntityReference();
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
        
        _dataService.Add(user);
        _dataService.AuthenticatedUser = user.ToEntityReference();
        return user.ToEntityReference();
    }
}