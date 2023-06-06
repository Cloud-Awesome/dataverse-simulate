using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.Interfaces;

public interface ISimulatorOptions
{
    public IClockSimulator? ClockSimulator { get; set; }
    
    public Entity? AuthenticatedUser { get; set; }
}