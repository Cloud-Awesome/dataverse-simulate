using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using CloudAwesome.Xrm.Simulate.ServiceProviders;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.PluginTelemetry;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate;

public static class ServiceProviderSimulator
{
    private static readonly MockedEntityDataService DataService = new();
    private static readonly MockedLoggingService LoggingService = new();
    private static readonly MockedTelemetryService TelemetryService = new();
    private static readonly SimulatorAuditService SimulatorAuditService = new();
    
    public static IServiceProvider Simulate(this IServiceProvider serviceProvider,
        ISimulatorOptions? options = null)
    {
        DataService.Reinitialise();
        LoggingService.Clear();
        TelemetryService.Clear();
        SimulatorAuditService.Clear();
        
        var localServiceProvider = Substitute.For<IServiceProvider>();
        
        // TODO - Process the rest of the simulator options
        DataService.FakeServiceFailureSettings = options?.FakeServiceFailureSettings;
        DataService.ExecutionContext = options?.PluginExecutionContextMock;
        
        localServiceProvider.GetService(Arg.Any<Type>())
            .Returns(callInfo =>
            {
                var argType = callInfo.Arg<Type>();
                return argType switch
                {
                    _ when argType == typeof(IPluginExecutionContext) => 
                        PluginExecutionContextSimulator.Create(DataService, options),
                    _ when argType == typeof(IOrganizationServiceFactory) => 
                        OrganisationServiceFactorySimulator.Create(DataService, options),
                    _ when argType == typeof(ITracingService) => 
                        TracingServiceSimulator.Create(DataService, LoggingService, options),
                    _ when argType == typeof(ILogger) => 
                        TelemetrySimulator.Create(DataService, TelemetryService, options),
                    _ when argType == typeof(IServiceEndpointNotificationService) => 
                        ServiceEndpointNotificationSimulator.Create(DataService, options),
                    _ => throw new ArgumentException("Type of Service requested is not supported")
                };
            });
        
        return localServiceProvider;
    }

    public static ServiceProviderSimulated Simulated(this IServiceProvider serviceProvider)
    {
        return new ServiceProviderSimulated(DataService, LoggingService, TelemetryService, SimulatorAuditService);
    }
}