using System.Text.RegularExpressions;
using CloudAwesome.Xrm.Simulate.DataServices;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class LikeConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.Like;

    public bool Evaluate(Entity entity, ConditionExpression condition, MockedEntityDataService dataService)
    {
        var attributeValue = entity.GetAttributeValue<string>(condition.AttributeName);
        if (attributeValue == null) return false;
        var pattern = 
            "^" + Regex.Escape(
                condition.Values[0].ToString())
                    .Replace("\\*", ".*")
                    .Replace("\\?", ".") 
                + "$";
        return Regex.IsMatch(attributeValue, pattern);
    }
}