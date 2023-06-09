using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate;

public class SimulatorOptions: ISimulatorOptions
{
    public IClockSimulator? ClockSimulator { get; set; }
    public Entity? AuthenticatedUser { get; set; }
    
    public Dictionary<string, Dictionary<ProcessorMessage, IEntityProcessor>>? EntityProcessors { get; set; }

    public enum ProcessorMessage
    {
        Create = 0,
        Update = 1
    }
}