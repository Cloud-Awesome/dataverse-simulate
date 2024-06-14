using CloudAwesome.Xrm.Simulate.Interfaces;

namespace CloudAwesome.Xrm.SamplePlugins.Test;

public class MockSystemTime: IClockSimulator
{
	public MockSystemTime(DateTime now)
	{
		Now = now;
	}
	
	public DateTime Now { get; }
}