using CloudAwesome.Xrm.Simulate.Interfaces;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate;

public static class ServiceProviderSimulator
{
    private static readonly IServiceProvider ServiceProvider = Substitute.For<IServiceProvider>();
    
    public static IServiceProvider Simulate(this IServiceProvider serviceProvider,
        ISimulatorOptions? options = null)
    {
        /* TODO - Set up the other mocks for ITracingService, IPluginExecutionContext, and IOrganizationServiceFactory */
        
        return ServiceProvider;
    }
}