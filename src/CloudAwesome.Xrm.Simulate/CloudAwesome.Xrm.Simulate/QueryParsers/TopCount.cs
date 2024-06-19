using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.QueryParsers;

public static class TopCount
{
	public static IQueryable<Entity> Apply(int? topCount, IEnumerable<Entity> records)
	{
		return topCount.HasValue ? 
			records.Take(topCount.Value).AsQueryable() : 
			records.AsQueryable();
	}
}