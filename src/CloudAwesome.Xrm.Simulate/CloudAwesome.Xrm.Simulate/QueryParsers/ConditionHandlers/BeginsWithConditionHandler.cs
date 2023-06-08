using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class BeginsWithConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.BeginsWith;

    public bool Evaluate(Entity entity, ConditionExpression condition)
    {
        var attributeValue = entity.GetAttributeValue<string>(condition.AttributeName);
        return attributeValue != null && attributeValue.StartsWith(condition.Values[0].ToString());
    }
}