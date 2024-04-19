using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using CloudAwesome.Xrm.Simulate.ServiceProviders;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.PluginTelemetry;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate;

public static class ServiceProviderSimulator
{
    private static MockedEntityDataService _dataService = null!;
    
    public static IServiceProvider Simulate(this IServiceProvider serviceProvider,
        ISimulatorOptions? options = null)
    {
        _dataService = new MockedEntityDataService();
        var localServiceProvider = Substitute.For<IServiceProvider>();
        
        // TODO - Process the rest of the simulator options
        _dataService.FakeServiceFailureSettings = options?.FakeServiceFailureSettings;
        _dataService.ExecutionContext = options?.PluginExecutionContextMock;
        
        localServiceProvider.GetService(Arg.Any<Type>())
            .Returns(callInfo =>
            {
                var argType = callInfo.Arg<Type>();
                return argType switch
                {
                    _ when argType == typeof(IPluginExecutionContext) => 
                        PluginExecutionContextSimulator.Create(_dataService, options),
                    _ when argType == typeof(IOrganizationServiceFactory) => 
                        OrganisationServiceFactorySimulator.Create(_dataService, options),
                    _ when argType == typeof(ITracingService) => 
                        TracingServiceSimulator.Create(_dataService, options),
                    _ when argType == typeof(ILogger) => 
                        TelemetrySimulator.Create(_dataService, options),
                    _ when argType == typeof(IServiceEndpointNotificationService) => 
                        ServiceEndpointNotificationSimulator.Create(_dataService, options),
                    _ => throw new ArgumentException("Type of Service requested is not supported")
                };
            });
        
        return localServiceProvider;
    }
    
    public static MockedEntityDataService Data(this IServiceProvider organizationService)
    {
        return _dataService;
    }
}