using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using CloudAwesome.Xrm.Simulate.ServiceProviders;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.PluginTelemetry;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate;

public static class ServiceProviderSimulator
{
    public static IServiceProvider Simulate(this IServiceProvider serviceProvider,
        ISimulatorOptions? options = null)
    {
        var dataServices = new MockedEntityDataService();
        var localServiceProvider = Substitute.For<IServiceProvider>();
        
        // TODO - Process the rest of the simulator options
        dataServices.FakeServiceFailureSettings = options?.FakeServiceFailureSettings;
        dataServices.ExecutionContext = options?.PluginExecutionContextMock;
        
        localServiceProvider.GetService(Arg.Any<Type>())
            .Returns(callInfo =>
            {
                var argType = callInfo.Arg<Type>();
                return argType switch
                {
                    _ when argType == typeof(IPluginExecutionContext) => PluginExecutionContextSimulator.Create(options),
                    _ when argType == typeof(IOrganizationServiceFactory) => OrganisationServiceFactorySimulator.Create(options),
                    _ when argType == typeof(ITracingService) => TracingServiceSimulator.Create(options),
                    _ when argType == typeof(ILogger) => TelemetrySimulator.Create(options),
                    _ when argType == typeof(IServiceEndpointNotificationService) => ServiceEndpointNotificationSimulator.Create(options),
                    _ => throw new ArgumentException("Type of Service requested is not supported")
                };
            });
        
        return localServiceProvider;
    }
}