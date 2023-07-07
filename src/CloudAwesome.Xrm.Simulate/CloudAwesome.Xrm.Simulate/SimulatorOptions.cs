using CloudAwesome.Xrm.Simulate.Interfaces;
using CloudAwesome.Xrm.Simulate.ServiceRequests;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate;

public class SimulatorOptions: ISimulatorOptions
{
    public IClockSimulator? ClockSimulator { get; set; }
    public Entity? AuthenticatedUser { get; set; }
    public Dictionary<ProcessorType, IEntityProcessor>? EntityProcessors { get; set; }
}