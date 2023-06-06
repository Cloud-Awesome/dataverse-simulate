using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers;

public static class Filter
{
    public static IQueryable<Entity> Apply(FilterExpression filter, IQueryable<Entity> records)
    {
        // TODO - Support AND/OR multiples
        if (filter.Conditions == null || filter.Conditions.Count == 0)
        {
            return records;
        }

        foreach (var condition in filter.Conditions)
        {
            var handler = ConditionHandlerFactory.GetHandler(condition.Operator);
            records = records.Where(entity => handler.Evaluate(entity, condition));
        }

        return records;
    }
}