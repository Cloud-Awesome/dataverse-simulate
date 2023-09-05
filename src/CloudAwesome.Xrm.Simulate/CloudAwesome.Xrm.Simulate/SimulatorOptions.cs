using CloudAwesome.Xrm.Simulate.DataStores;
using CloudAwesome.Xrm.Simulate.Interfaces;
using CloudAwesome.Xrm.Simulate.ServiceProviders;
using CloudAwesome.Xrm.Simulate.ServiceRequests;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate;

public class SimulatorOptions: ISimulatorOptions
{
    public IClockSimulator? ClockSimulator { get; set; }
    public Entity? AuthenticatedUser { get; set; }
    public Dictionary<ProcessorType, IEntityProcessor>? EntityProcessors { get; set; }
    public PluginExecutionContextMock? PluginExecutionContextMock { get; set; }
    
    public FakeServiceFailureSettings? FakeServiceFailureSettings { get; set; }


    // TODO - Inject things like business unit and organisation etc.
}