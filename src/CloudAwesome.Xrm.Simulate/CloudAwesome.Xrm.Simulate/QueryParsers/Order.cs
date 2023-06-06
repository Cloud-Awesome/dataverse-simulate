using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers;

public static class Order
{
    public static IQueryable<Entity> Apply(IList<OrderExpression> orders, IQueryable<Entity> records)
    {
        if (orders == null || orders.Count == 0)
        {
            return records;
        }

        IOrderedQueryable<Entity> orderedRecords = null;

        for (var i = 0; i < orders.Count; i++)
        {
            var order = orders[i];

            if (i == 0)
            {
                orderedRecords = order.OrderType == OrderType.Ascending
                    ? records.OrderBy(entity => entity.GetAttributeValue<object>(order.AttributeName))
                    : records.OrderByDescending(entity => entity.GetAttributeValue<object>(order.AttributeName));
            }
            else
            {
                orderedRecords = order.OrderType == OrderType.Ascending
                    ? orderedRecords.ThenBy(entity => entity.GetAttributeValue<object>(order.AttributeName))
                    : orderedRecords.ThenByDescending(entity => entity.GetAttributeValue<object>(order.AttributeName));
            }

            return orderedRecords;
        }
            
        return records;
    }
}