using CloudAwesome.Xrm.Simulate.ServiceRequests;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.Interfaces;

public interface ISimulatorOptions
{
    public IClockSimulator? ClockSimulator { get; set; }
    
    public Entity? AuthenticatedUser { get; set; }
    
    public Dictionary<ProcessorType, IEntityProcessor>? EntityProcessors { get; set; }
}