using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers;

public static class Filter
{
    public static IQueryable<Entity> Apply(FilterExpression filter, IQueryable<Entity> records)
    {
        // A copy of the original records.
        var originalRecords = records;
    
        // List to hold the results of each sub-filter.
        List<IQueryable<Entity>> subFilterResults = new List<IQueryable<Entity>>();

        // First, handle the conditions at this level of the filter.
        if (filter.Conditions != null && filter.Conditions.Count > 0)
        {
            foreach (var condition in filter.Conditions)
            {
                var handler = ConditionHandlerFactory.GetHandler(condition.Operator);
                var conditionResults = originalRecords.Where(entity => handler.Evaluate(entity, condition));
                subFilterResults.Add(conditionResults);
            }
        }

        // Then, handle all the sub-filters.
        if (filter.Filters is { Count: > 0 })
        {
            // Process each sub-filter.
            foreach (var subFilter in filter.Filters)
            {
                var subFilterResult = Apply(subFilter, originalRecords);
                subFilterResults.Add(subFilterResult);
            }
        }

        // Combine the results according to the logical operator of this filter.
        if (filter.FilterOperator == LogicalOperator.And)
        {
            // Start with all records, then intersect with each sub-filter result.
            records = originalRecords;
            foreach (var result in subFilterResults)
            {
                records = records.Intersect(result);
            }
        }
        else  // LogicalOperator.Or
        {
            // Start with no records, then union with each sub-filter result.
            records = records.Take(0);
            foreach (var result in subFilterResults)
            {
                records = records.Union(result);
            }
        }

        return records;
    }
}