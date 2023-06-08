using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class EndsWithConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.EndsWith;

    public bool Evaluate(Entity entity, ConditionExpression condition)
    {
        var attributeValue = entity.GetAttributeValue<string>(condition.AttributeName);
        return attributeValue != null && attributeValue.EndsWith(condition.Values[0].ToString());
    }
}