using CloudAwesome.Xrm.Simulate.Interfaces;

namespace CloudAwesome.Xrm.Simulate;

public class MockSystemTime(DateTime now) : IClockSimulator
{
    public DateTime Now { get; } = now;
}