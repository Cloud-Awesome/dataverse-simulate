using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate.ServiceProviders;

public static class OrganisationServiceFactorySimulator
{
    private static readonly IOrganizationService Service = null!;
    
    public static IOrganizationServiceFactory? Create(MockedEntityDataService dataService, ISimulatorOptions? options)
    {
        if (dataService.FakeServiceFailureSettings is { OrganizationServiceFactory: true })
        {
            return null;
        }
        
        var serviceFactory = Substitute.For<IOrganizationServiceFactory>();

        serviceFactory.CreateOrganizationService(Arg.Any<Guid>())
            .Returns(x => Service.Simulate(options));

        return serviceFactory;
    }
}