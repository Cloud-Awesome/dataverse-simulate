using System.Xml;
using CloudAwesome.Xrm.Simulate.DataServices;
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
    public static IEnumerable<Entity> Parse(FetchExpression query, Dictionary<string, 
        List<Entity>> data, MockedEntityDataService dataService)
    {
        if (query == null || query.Query == null)
        {
            return Enumerable.Empty<Entity>();
        }

        var queryExpression = ConvertFetchXmlToQueryExpression(query.Query);
        return QueryExpressionParser.Parse(queryExpression, data, dataService);
    }
    
    public static QueryExpression ConvertFetchXmlToQueryExpression(string fetchXml)
    {
        var doc = new XmlDocument();
        doc.LoadXml(fetchXml);

        var fetchNode = doc.DocumentElement;
        if (fetchNode == null) return new QueryExpression();
        
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

            if (int.TryParse(fetchNode.Attributes["top"]?.Value, out int topCount))
            {
                query.TopCount = topCount;
            }

            if (bool.TryParse(fetchNode.Attributes["distinct"]?.Value, out bool distinct))
            {
                query.Distinct = distinct;
            }

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
            // TODO - Return the correct exception CRM would throw if it wasn't a valid Fetch query
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
            // Fetch uses '%' for multiple characters, replace with '*' for RegEx
            valueNode.Value = valueNode.Value.Replace('%', '*');
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
        { "ge", ConditionOperator.GreaterEqual },
        { "gt", ConditionOperator.GreaterThan },
        { "last-seven-days", ConditionOperator.Last7Days},
        { "last-week", ConditionOperator.LastWeek},
        { "last-x-days", ConditionOperator.LastXDays},
        { "last-x-hours", ConditionOperator.LastXHours},
        { "last-x-months", ConditionOperator.LastXMonths},
        { "last-x-weeks", ConditionOperator.LastXWeeks},
        { "last-x-years", ConditionOperator.LastXYears},
        { "last-year", ConditionOperator.LastYear},
        { "le", ConditionOperator.LessEqual },
        { "lt", ConditionOperator.LessThan },
        { "like", ConditionOperator.Like },
        { "next-week", ConditionOperator.NextWeek },
        { "next-x-days", ConditionOperator.NextXDays },
        { "next-x-hours", ConditionOperator.NextXHours },
        { "next-x-months", ConditionOperator.NextXMonths },
        { "next-x-weeks", ConditionOperator.NextXWeeks },
        { "next-x-years", ConditionOperator.NextXYears },
        { "next-year", ConditionOperator.NextYear },
        { "ne", ConditionOperator.NotEqual },
        { "not-like", ConditionOperator.NotLike },
        { "not-null", ConditionOperator.NotNull },
        { "null", ConditionOperator.Null },
        { "on", ConditionOperator.On },
        { "on-or-after", ConditionOperator.OnOrAfter },
        { "on-or-before", ConditionOperator.OnOrBefore },
        { "this-month", ConditionOperator.ThisMonth },
        { "this-week", ConditionOperator.ThisWeek },
        { "this-year", ConditionOperator.ThisYear },
        { "today", ConditionOperator.Today },
        { "tomorrow", ConditionOperator.Tomorrow },
        { "yesterday", ConditionOperator.Yesterday },
        
        // etc., add all needed mappings here
    };
}