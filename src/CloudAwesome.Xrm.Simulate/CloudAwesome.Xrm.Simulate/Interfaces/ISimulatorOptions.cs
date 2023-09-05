using CloudAwesome.Xrm.Simulate.DataStores;
using CloudAwesome.Xrm.Simulate.ServiceProviders;
using CloudAwesome.Xrm.Simulate.ServiceRequests;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.Interfaces;

public interface ISimulatorOptions
{
    public IClockSimulator? ClockSimulator { get; set; }
    
    public Entity? AuthenticatedUser { get; set; }
    
    public Dictionary<ProcessorType, IEntityProcessor>? EntityProcessors { get; set; }
    
    public PluginExecutionContextMock? PluginExecutionContextMock { get; set; }
    
    public FakeServiceFailureSettings? FakeServiceFailureSettings { get; set; }
}