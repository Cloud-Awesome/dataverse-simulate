using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class NotEqualConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.NotEqual;

    public bool Evaluate(Entity entity, ConditionExpression condition)
    {
        var attributeValue = entity.GetAttributeValue<object>(condition.AttributeName);
        return !Equals(attributeValue, condition.Values[0]);
    }
}