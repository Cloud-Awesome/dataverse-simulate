using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate.ServiceProviders;

public static class OrganisationServiceFactorySimulator
{
    public static readonly IOrganizationService Service = null!;
    
    public static IOrganizationServiceFactory Create(ISimulatorOptions? options)
    {
        var serviceFactory = Substitute.For<IOrganizationServiceFactory>();

        serviceFactory.CreateOrganizationService(Arg.Any<Guid>())
            .Returns(x => Service.Simulate(options));

        return serviceFactory;
    }
}