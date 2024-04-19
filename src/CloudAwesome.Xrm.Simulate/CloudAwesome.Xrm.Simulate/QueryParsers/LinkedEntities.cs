using CloudAwesome.Xrm.Simulate.DataServices;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers;

public static class LinkedEntities
{
    public static IQueryable<Entity> Apply(List<LinkEntity>? linkedEntities, List<Entity> records, 
        Dictionary<string, List<Entity>> data, MockedEntityDataService dataService)
    {
        if (linkedEntities == null || !linkedEntities.Any())
        {
            return records.AsQueryable();
        }

        foreach (var linkedEntity in linkedEntities)
        {
            if (!data.ContainsKey(linkedEntity.LinkToEntityName))
            {
                continue;
            }

            var linkedRecords = data[linkedEntity.LinkToEntityName];

            linkedRecords = Filter.Apply(linkedEntity.LinkCriteria, linkedRecords.AsQueryable(), dataService).ToList();
            linkedRecords = Apply(linkedEntity.LinkEntities.ToList(), linkedRecords, data, dataService).ToList();
            records = JoinEntities(records, linkedRecords, 
                linkedEntity.LinkFromAttributeName, linkedEntity.LinkToAttributeName,
                linkedEntity.EntityAlias).ToList();
        }

        return records.AsQueryable();
    }

    private static object GetJoinKeyValue(object value)
    {
        return value is EntityReference entityReference ? entityReference.Id : value;
    }

    private static IEnumerable<Entity> JoinEntities(IEnumerable<Entity> primaryEntities, 
        IEnumerable<Entity> linkedEntities, string primaryKey, string linkedKey, string alias)
    {
        var result = from primary in primaryEntities
            join linked in linkedEntities
                on GetJoinKeyValue(primary.Attributes[primaryKey]) equals GetJoinKeyValue(linked.Attributes[linkedKey])
            select MergeEntities(primary, linked, alias);

        return result;
    }

    private static Entity MergeEntities(Entity primaryEntity, Entity linkedEntity, string? alias)
    {
        foreach (var attribute in linkedEntity.Attributes)
        {
            var attributeKey = alias is null ? attribute.Key : $"{alias}.{attribute.Key}";
            primaryEntity.Attributes[attributeKey] = attribute.Value;
        }

        return primaryEntity;
    }

}