using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers;

public static class Columns
{
    public static IQueryable<Entity> Apply(ColumnSet columnSet, IEnumerable<Entity> records)
    {
        if (columnSet == null || columnSet.AllColumns || 
            columnSet.Columns.Count == 0)
        {
            return records.AsQueryable();
        }

        return records.Select(entity =>
        {
            var e = new Entity(entity.LogicalName) { Id = entity.Id };
            foreach (var column in columnSet.Columns)
            {
                e[column] = entity[column];
            }
            return e;
        }).AsQueryable();
    }
        
}