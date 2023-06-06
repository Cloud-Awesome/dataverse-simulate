using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.DataStores;

internal static class EntityExtensions
{
    /// <summary>
    /// If the attribute exists in the entity and is not null, set a value
    /// </summary>
    /// <param name="entity">The entity instance to update</param>
    /// <param name="attributeName">The schema name of the attribute to update</param>
    /// <param name="value">The value of the attribute to update</param>
    public static void SetAttributeIfEmpty(this Entity entity, string attributeName, object value)
    {
        var containsKey = entity.Contains(attributeName);
        if (!containsKey || containsKey && entity[attributeName] == null)
        {
            entity[attributeName] = value;
        }
    }

    /// <summary>
    /// Copies/overwrites the value of one attribute from a source attribute to a target attribute if the source is populated
    /// </summary>
    /// <param name="entity">The entity instance to update</param>
    /// <param name="sourceAttributeName">The attribute in the entity to source the value</param>
    /// <param name="targetAttributeName">The attribute to update with the source value</param>
    public static void SetAttributeFromSourceIfPopulated(this Entity entity,
        string targetAttributeName, string sourceAttributeName)
    {
        var containsSourceKey = entity.Contains(sourceAttributeName);
        var containsTargetKey = entity.Contains(targetAttributeName);

        if (containsSourceKey && containsTargetKey && entity[sourceAttributeName] != null)
        {
            entity[targetAttributeName] = entity[sourceAttributeName];
        }
            
    }
}