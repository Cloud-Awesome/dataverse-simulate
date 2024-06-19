using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.QueryParsers;

public static class Distinct
{
	public static IQueryable<Entity> Apply(bool distinct, IEnumerable<Entity> records)
	{
		return distinct ? 
			records.Distinct(new EntityComparer()).AsQueryable() : 
			records.AsQueryable();
	}
}