﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers;

public static class QueryExpressionParser
{
    public static IEnumerable<Entity> Parse(QueryExpression query, Dictionary<string, List<Entity>> data)
    {
        if (query == null || data == null || !data.ContainsKey(query.EntityName))
        {
            return Enumerable.Empty<Entity>();
        }

        var records = data[query.EntityName].AsQueryable();
            
        records = Filter.Apply(query.Criteria, records);
        records = Order.Apply(query.Orders, records);
        records = Columns.Apply(query.ColumnSet, records);

        return records;
    }
}