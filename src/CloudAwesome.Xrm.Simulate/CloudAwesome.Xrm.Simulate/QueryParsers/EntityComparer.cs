using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.QueryParsers;

public class EntityComparer : IEqualityComparer<Entity>
{
	public bool Equals(Entity x, Entity y)
	{
		if (x == null || y == null) return false;

		if (x.Attributes.Count != y.Attributes.Count) return false;

		foreach (var attribute in x.Attributes)
		{
			if (!y.Attributes.ContainsKey(attribute.Key)) return false;

			var xValue = attribute.Value;
			var yValue = y.Attributes[attribute.Key];

			if (xValue == null && yValue != null) return false;
			if (xValue != null && yValue == null) return false;
			if (xValue != null && !xValue.Equals(yValue)) return false;
		}

		return true;
	}

	public int GetHashCode(Entity obj)
	{
		if (obj == null) return 0;

		int hash = 17;
		foreach (var attribute in obj.Attributes)
		{
			hash = hash * 31 + (attribute.Key.GetHashCode());
			if (attribute.Value != null)
			{
				hash = hash * 31 + attribute.Value.GetHashCode();
			}
		}
		return hash;
	}
}
