using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using NSubstitute;

namespace CloudAwesome.Xrm.Simulate.ServiceProviders;

public static class TracingServiceSimulator
{
    public static ITracingService Create(ISimulatorOptions? options)
    {
        var tracingService = Substitute.For<ITracingService>();

        tracingService
            .When(x => x.Trace(Arg.Any<string>()))
            .Do(t =>
            {
                var trace = t.Arg<string>();
                var loggingService = new MockedLoggingService();
                loggingService.Add(trace);
            });
        
        tracingService
            .When(x => x.Trace(Arg.Any<string>(), Arg.Any<object[]>()))
            .Do(t =>
            {
                var trace = t.Arg<string>();
                var args = t.Arg<object[]>();
                var loggingService = new MockedLoggingService();
                loggingService.Add(trace, args);
            });

        return tracingService;
    }
}