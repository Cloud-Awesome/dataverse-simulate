using System.Xml;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// N.B. Filters and LinkEntity currently only work if you've included the attributes in the ColumnSet
/// </remarks>
public static class FetchExpressionParser
{
    public static IEnumerable<Entity> Parse(FetchExpression query, Dictionary<string, List<Entity>> data)
    {
        if (query == null || query.Query == null)
        {
            return Enumerable.Empty<Entity>();
        }

        var queryExpression = ConvertFetchXmlToQueryExpression(query.Query);
        return QueryExpressionParser.Parse(queryExpression, data);
    }
    
    public static QueryExpression? ConvertFetchXmlToQueryExpression(string fetchXml)
    {
        var doc = new XmlDocument();
        doc.LoadXml(fetchXml);

        var fetchNode = doc.DocumentElement;
        if (fetchNode == null) return null;

        try
        {
            var entityNode = fetchNode.SelectSingleNode("entity");
            var query = new QueryExpression(entityNode.Attributes["name"]?.Value)
            {
                ColumnSet = new ColumnSet(
                    entityNode?
                        .SelectNodes("attribute")?.Cast<XmlNode>()
                        .Select(x => x.Attributes?["name"]?.Value)
                        .ToArray())
            };

            if (entityNode?.SelectSingleNode("filter") is not null)
            {
                query.Criteria = ParseFilter(entityNode.SelectSingleNode("filter"));
            }
            
            if (entityNode?.SelectSingleNode("order") is not null)
            {
                foreach (XmlNode orderNode in entityNode.SelectNodes("order"))
                {
                    var order = ParseOrder(orderNode);
                    query.Orders.Add(order);
                }
            }

            if (entityNode?.SelectNodes("link-entity")?.Count > 0)
            {
                foreach (XmlNode linkEntityNode in entityNode?.SelectNodes("link-entity"))
                {
                    query.LinkEntities.Add(ParseLinkEntity(linkEntityNode,
                        entityNode.Attributes["name"]?.Value));
                }    
            }
            
            return query;
        }
        catch (Exception e)
        {
            // Return the correct exception CRM would throw if it wasn't a valid Fetch query
            throw;
        }
    }
    
    private static FilterExpression ParseFilter(XmlNode filterNode)
    {
        var filter = new FilterExpression();

        if (filterNode != null)
        {
            foreach (XmlNode conditionNode in filterNode.SelectNodes("condition"))
            {
                var condition = ParseCondition(conditionNode);
                filter.Conditions.Add(condition);
            }
        }

        return filter;
    }
    
    private static ConditionExpression ParseCondition(XmlNode conditionNode)
    {
        var attributeName = conditionNode.Attributes["attribute"].Value;
        var operatorName = conditionNode.Attributes["operator"].Value;
        var valueNode = conditionNode.Attributes["value"];

        object value = null;
        if (valueNode != null)
        {
            value = valueNode.Value;
        }

        // Try to map the operator from the FetchXML to a ConditionOperator, if mapping not found, default to Equal?
        ConditionOperator conditionOperator = OperatorMappings.TryGetValue(operatorName, out ConditionOperator op)
            ? op
            : ConditionOperator.Equal;

        var condition = new ConditionExpression(attributeName, conditionOperator, value);

        return condition;
    }
    
    private static OrderExpression ParseOrder(XmlNode orderNode)
    {
        var attributeName = orderNode.Attributes["attribute"].Value;
        var orderType = (OrderType)Enum.Parse(typeof(OrderType), orderNode.Attributes["descending"]?.Value == "true" ? "Descending" : "Ascending", true);

        return new OrderExpression(attributeName, orderType);
    }

    
    private static LinkEntity? ParseLinkEntity(XmlNode linkEntityNode, string baseEntityName)
    {
        if (linkEntityNode.Attributes == null) return null;

        var linkEntity = new LinkEntity()
        {
            LinkFromEntityName = baseEntityName,
            LinkFromAttributeName = linkEntityNode.Attributes["from"]?.Value,
            LinkToEntityName = linkEntityNode.Attributes["name"]?.Value,
            LinkToAttributeName = linkEntityNode.Attributes["to"]?.Value,
            EntityAlias = linkEntityNode.Attributes["alias"]?.Value
        };

        foreach (XmlNode attrNode in linkEntityNode.SelectNodes("attribute"))
        {
            linkEntity.Columns.AddColumns(attrNode.Attributes["name"].Value);
        }

        foreach (XmlNode linkEntityChildNode in linkEntityNode.SelectNodes("link-entity"))
        {
            linkEntity.LinkEntities.Add(ParseLinkEntity(linkEntityChildNode, 
                linkEntity.LinkToEntityName));
        }

        foreach (XmlNode orderNode in linkEntityNode.SelectNodes("order"))
        {
            var attributeName = orderNode.Attributes["attribute"].Value;
            var orderType = (OrderType)Enum.Parse(typeof(OrderType), orderNode.Attributes["descending"]?.Value == "true" ? "Descending" : "Ascending", true);
            linkEntity.Orders.Add(new OrderExpression(attributeName, orderType));
        }

        linkEntity.LinkCriteria = ParseFilter(linkEntityNode.SelectSingleNode("filter"));
    
        return linkEntity;
    }
    
    private static readonly Dictionary<string, ConditionOperator> OperatorMappings = new Dictionary<string, ConditionOperator>(StringComparer.OrdinalIgnoreCase)
    {
        { "eq", ConditionOperator.Equal },
        { "ne", ConditionOperator.NotEqual },
        { "gt", ConditionOperator.GreaterThan },
        { "ge", ConditionOperator.GreaterEqual },
        { "lt", ConditionOperator.LessThan },
        { "le", ConditionOperator.LessEqual },
        { "like", ConditionOperator.Like },
        { "not-like", ConditionOperator.NotLike },
        // etc., add all needed mappings here
    };
}