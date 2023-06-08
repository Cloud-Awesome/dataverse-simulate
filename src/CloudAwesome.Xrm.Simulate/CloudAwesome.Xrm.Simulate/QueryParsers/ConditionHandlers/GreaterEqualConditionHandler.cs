using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Simulate.QueryParsers.ConditionHandlers;

public class GreaterEqualConditionHandler : IConditionHandler
{
    public ConditionOperator Operator => ConditionOperator.GreaterEqual;

    public bool Evaluate(Entity entity, ConditionExpression condition)
    {
        var attributeValue = entity.GetAttributeValue<IComparable>(condition.AttributeName);
        return attributeValue.CompareTo(condition.Values[0]) >= 0;
    }
}