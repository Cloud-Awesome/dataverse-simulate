using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate.ServiceProviders;

public static class TracingServiceSimulator
{
    private static MockedLoggingService _mockedLoggingService = null!;
    
    public static ITracingService? Create(MockedEntityDataService dataService, MockedLoggingService loggingService, 
        ISimulatorOptions? options)
    {
        if (dataService.FakeServiceFailureSettings is { TracingService: true })
        {
            return null;
        }
        
        var tracingService = Substitute.For<ITracingService>();
        _mockedLoggingService = loggingService;

        tracingService
            .When(x => x.Trace(Arg.Any<string>(), Arg.Any<object[]>()))
            .Do(t =>
            {
                var trace = t.Arg<string>();
                var args = t.Arg<object[]>();
                _mockedLoggingService.Add(trace, args);
            });

        return tracingService;
    }
}