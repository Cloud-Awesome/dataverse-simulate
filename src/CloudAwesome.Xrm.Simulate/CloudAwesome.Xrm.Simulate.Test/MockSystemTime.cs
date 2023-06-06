using System;
using CloudAwesome.Xrm.Simulate.Interfaces;

namespace CloudAwesome.Xrm.Simulate.Test;

public class MockSystemTime: IClockSimulator
{
    public MockSystemTime(DateTime now)
    {
        Now = now;
    }

    public DateTime Now { get; }
}