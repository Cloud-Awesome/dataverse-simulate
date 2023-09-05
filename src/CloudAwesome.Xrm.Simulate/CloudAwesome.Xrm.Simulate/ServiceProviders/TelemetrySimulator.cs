using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk.PluginTelemetry;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate.ServiceProviders;

public static class TelemetrySimulator
{
    private static readonly MockedTelemetryService Telemetry = new MockedTelemetryService();
    
    public static ILogger? Create(ISimulatorOptions? options)
    {
        var dataService = new MockedEntityDataService();
        if (dataService.FakeServiceFailureSettings is { TelemetryService: true })
        {
            return null;
        }
        
        var telemetryService = Substitute.For<ILogger>();
        
        telemetryService
            .When(x => x.Log(Arg.Any<LogLevel>(), Arg.Any<string>(), Arg.Any<object[]>()))
            .Do(callInfo =>
            {
                var logLevel = callInfo.Arg<LogLevel>();
                var message = callInfo.Arg<string>();
                var parameters = callInfo.Arg<object[]>();
                
                Telemetry.Add(logLevel, message, parameters);
            });
        
        ConfigureTelemetryMock(telemetryService, telemetryService.LogCritical, LogLevel.Critical);
        ConfigureTelemetryMock(telemetryService, telemetryService.LogError, LogLevel.Error);
        ConfigureTelemetryMock(telemetryService, telemetryService.LogWarning, LogLevel.Warning);
        ConfigureTelemetryMock(telemetryService, telemetryService.LogInformation, LogLevel.Information);
        ConfigureTelemetryMock(telemetryService, telemetryService.LogTrace, LogLevel.Trace);
        ConfigureTelemetryMock(telemetryService, telemetryService.LogDebug, LogLevel.Debug);
        
        return telemetryService;
    }

    private static void ConfigureTelemetryMock(ILogger telemetryService, Action<string, object[]> logAction, LogLevel logLevel)
    {
        telemetryService
            .When(x => logAction(Arg.Any<string>(), Arg.Any<object[]>()))
            .Do(callInfo =>
            {
                var message = callInfo.Arg<string>();
                var parameters = callInfo.Arg<object[]>();
                Telemetry.Add(logLevel, message, parameters);
            });
    }
}